using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MacroBoard.View.Themes;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<WorkflowView> FavWorkflows = new();
        private List<WorkflowView> Workflows = new();
        private List<WorkflowView> WorkflowsSearchs = new();
        bool isEdition = false;
        bool isInsearch = false;

        public App CurrentApplication { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitWorkflows();
        }
        private void InitWorkflows()
        {
            Workflows = Serialization.getWorkFlowsFromJson();
            FavWorkflows = Serialization.getFavsFromJson();
            foreach (WorkflowView workflowView in Workflows)
            {
                CreateButton(workflowView, false);

            }
            foreach (WorkflowView FavWorkflows in FavWorkflows)
            {
                CreateButton(FavWorkflows, true);

            }
            if (Workflows.Count < 10)
            {
                AddAddButton(Workflows);

            }
        }
        private void CreateButton(WorkflowView workflowView, bool isFav)
        {
            if (isFav)
            {
                workflowView.Btn_Delete.Visibility = Visibility.Hidden;
                workflowView.Btn_Main.Click += Button_Click_Fav;
                workflowView.Btn_Fav.Click += OnClick_Delete_Fav;
            }
            else
            {
                workflowView.Btn_Delete.Click += OnClick_DeleteWorkflow;
                workflowView.Btn_Fav.Click += OnClick_Fav;
                workflowView.Btn_Main.Click += Button_Click;
            }
            if (!workflowView.CurrentworkFlow.imagePath.Equals(""))
            {
                workflowView.Btn_Main.Content = new Image
                {
                    Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.Absolute))
                };
            }
            if (isFav)
            {
                ListFav.Items.Add(workflowView.Content);
            }
            else
            {
                ListMacro.Items.Add(workflowView.Content);
            }

        }

        private void CreateButton(WorkflowView workflowView, bool isFav, int pos)
        {
            if (isFav)
            {
                workflowView.Btn_Delete.Visibility = Visibility.Hidden;
                workflowView.Btn_Main.Click += Button_Click_Fav;
                workflowView.Btn_Fav.Click += OnClick_Delete_Fav;
            }
            else
            {
                workflowView.Btn_Delete.Click += OnClick_DeleteWorkflow;
                workflowView.Btn_Fav.Click += OnClick_Fav;
                workflowView.Btn_Main.Click += Button_Click;
            }
            if (!workflowView.CurrentworkFlow.imagePath.Equals(""))
            {
                workflowView.Btn_Main.Content = new Image
                {
                    Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.Absolute))
                };
            }
            if (isFav)
            {
                ListFav.Items.Insert(pos, workflowView.Content);
            }
            else
            {
                ListMacro.Items.Insert(pos, workflowView.Content);
            }

        }

        private void AddWorkFlowWhileSearch(object sender, RoutedEventArgs e)
        {
            EditionWindow editionWindow = new();
            editionWindow.ShowDialog();

            if (editionWindow.DialogResult == true)
            {

                while (ListContainsName(editionWindow.WorkFlow.workflowName))
                {
                    editionWindow.WorkFlow.workflowName += 1;
                }
                WorkFlow wf = editionWindow.WorkFlow;


                string JsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\WFJSON\" + wf.workflowName + ".json";
                Serialization serialization = new Serialization(JsonPath);

                serialization.Serialize(wf);



                Workflows.Insert(Workflows.Count - 1, new(wf));
                Workflows[^2].Btn_Delete.Click += OnClick_DeleteWorkflow;
                Workflows[^2].Btn_Fav.Click += OnClick_Fav;
                Workflows[^2].Btn_Main.Click += Button_Click;
                if (!wf.imagePath.Equals(""))
                {
                    Workflows[^2].Btn_Main.Content = new Image
                    {
                        Source = new BitmapImage(new Uri(wf.imagePath, UriKind.Absolute))
                    };
                }

                if (Workflows.Count == 11 && Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
                {
                    Workflows.RemoveAt(Workflows.Count - 1);
                }
            }
            UpdateSearchList();

        }





        private void OnClick_Delete_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            Serialization.DeleteFAV(FavWorkflows[currentItemPos].CurrentworkFlow.workflowName);
            ListFav.Items.RemoveAt(currentItemPos);
            FavWorkflows.RemoveAt(currentItemPos);

        }


        private void OnClick_DeleteWorkflow(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);


            RemoveWorkflow(currentItemPos);
            if (isInsearch) { UpdateSearchList(); }
            else
            {
                if (Workflows.Count == 9 && !Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
                {
                    AddAddButton(Workflows);
                }
            }




        }

        private void AddAddButton(List<WorkflowView> workflowViews)
        {
            WorkFlow addButton = new("", "", new List<Block>());
            workflowViews.Add(new(addButton));
            workflowViews[^1].Btn_Delete.Visibility = Visibility.Hidden;
            workflowViews[^1].Btn_Fav.Visibility = Visibility.Hidden;
            workflowViews[^1].Btn_Main.Click += AddWorkFlow;
            workflowViews[^1].Btn_Main.Content
                            = new Image
                            {
                                Source = new BitmapImage(new Uri("../../../Resources/Button_WorkFlow_Add.png", UriKind.Relative))
                            };
            ListMacro.Items.Add(workflowViews[^1].Content);
        }


        private void RemoveWorkflow(int buttonCLickedPos)
        {
            if (isInsearch)

            {
                WorkflowView wfv = WorkflowsSearchs[buttonCLickedPos];
                buttonCLickedPos = 0;
                while (wfv.CurrentworkFlow.workflowName != Workflows[buttonCLickedPos].CurrentworkFlow.workflowName)
                {
                    buttonCLickedPos++;
                    if (buttonCLickedPos > 10)
                    {
                        return;
                    }
                }

            }


            int currentItemPosFav = 0;
            bool test = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!Workflows[buttonCLickedPos].CurrentworkFlow.workflowName.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow.workflowName))
                {
                    currentItemPosFav++;
                }
                else
                {
                    test = true;
                    break;
                }
            }
            if (test)
            {
                Serialization.DeleteFAV(FavWorkflows[currentItemPosFav].CurrentworkFlow.workflowName);
                FavWorkflows.RemoveAt(currentItemPosFav);
                ListFav.Items.RemoveAt(currentItemPosFav);
            }
            Serialization.DeleteWF(Workflows[buttonCLickedPos].CurrentworkFlow.workflowName);
            if (!isInsearch)
            {
                ListMacro.Items.RemoveAt(buttonCLickedPos);

            }
            Workflows.RemoveAt(buttonCLickedPos);
        }


        private void OnClick_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            if (!isInsearch)
            {
                AddFav(new WorkflowView(Workflows[currentItemPos].CurrentworkFlow));
            }
            else
            {
                AddFav(new WorkflowView(WorkflowsSearchs[currentItemPos].CurrentworkFlow));
            }
        }
        private void AddFav(WorkflowView newFav)
        {
            WorkFlow wf = newFav.CurrentworkFlow;
            if (FavWorkflows.Count < 5)
            {
                if (!ListContains(FavWorkflows, wf))
                {
                    newFav.Btn_Fav.Click += OnClick_Delete_Fav;
                    newFav.Btn_Main.Click += Button_Click_Fav;
                    newFav.Btn_Delete.Visibility = Visibility.Hidden;
                    if (!newFav.CurrentworkFlow.imagePath.Equals(""))
                    {
                        newFav.Btn_Main.Content
                            = new Image
                            {
                                Source = new BitmapImage(new Uri(newFav.CurrentworkFlow.imagePath, UriKind.Absolute))
                            };
                    }
                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + wf.workflowName + ".json");
                    serialization.Serialize(wf);
                    ListFav.Items.Add(newFav.Content);
                    FavWorkflows.Add(newFav);
                }
            }
        }

        private static bool ListContains(List<WorkflowView> workflowViews, WorkFlow workFlow)
        {
            bool contains = false;
            foreach (WorkflowView workflowView in workflowViews)
            {
                if (workflowView.CurrentworkFlow.workflowName.Equals(workFlow.workflowName))
                {
                    return true;
                }
            }
            return contains;
        }


        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Search.Text;
            WorkflowsSearchs = new();
            if (!searchText.Equals(""))
            {
                isInsearch = true;
                foreach (WorkflowView workFlowView in Workflows)
                {
                    if (workFlowView.CurrentworkFlow.workflowName.ToLower().Contains(searchText.ToLower()))
                    {
                        WorkflowsSearchs.Add(workFlowView);

                    }
                }
                if (Workflows.Count < 10)
                {
                    AddAddButton(WorkflowsSearchs);

                }

            }
            else
            {
                isInsearch = false;
                WorkflowsSearchs = Workflows;
            }

            ListMacro.Items.Clear();
            foreach (WorkflowView mac in WorkflowsSearchs)
            {
                ListMacro.Items.Add(mac.Content);
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);

            if (isInsearch)
            {

                ExecuteWorkflow((WorkflowsSearchs[currentItemPos].CurrentworkFlow), currentItemPos);





            }
            else
            {
                ExecuteWorkflow((Workflows[currentItemPos].CurrentworkFlow), currentItemPos);
            }
        }



        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            ExecuteWorkflowFav(FavWorkflows[currentItemPos].CurrentworkFlow, currentItemPos);
        }

        private void AddWorkFlow(object sender, RoutedEventArgs e)
        {
            if (isInsearch)
            {
                AddWorkFlowWhileSearch(sender, e);
            }
            else
            {
                EditionWindow editionWindow = new();
                editionWindow.ShowDialog();
                if (editionWindow.DialogResult == true)
                {

                    while (ListContainsName(editionWindow.WorkFlow.workflowName))
                    {
                        editionWindow.WorkFlow.workflowName += 1;
                    }
                    WorkFlow wf = editionWindow.WorkFlow;


                    string JsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\WFJSON\" + wf.workflowName + ".json";
                    Serialization serialization = new Serialization(JsonPath);

                    serialization.Serialize(wf);

                    MessageBox.Show("Serialization done ");
                    Workflows.Insert(Workflows.Count - 1, new(wf));
                    Workflows[^2].Btn_Delete.Click += OnClick_DeleteWorkflow;
                    Workflows[^2].Btn_Fav.Click += OnClick_Fav;
                    Workflows[^2].Btn_Main.Click += Button_Click;
                    if (!Workflows[^2].CurrentworkFlow.imagePath.Equals("") && !Workflows[^2].CurrentworkFlow.imagePath.Equals("Select folder"))
                    {
                        Workflows[^2].Btn_Main.Content
                     = new Image
                     {
                         Source = new BitmapImage(new Uri(Workflows[^2].CurrentworkFlow.imagePath, UriKind.Absolute))
                     };


                    }
                    if (Workflows.Count <= 10)
                    {
                        if (Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
                        {
                            ListMacro.Items.Insert(Workflows.Count - 2, Workflows[^2].Content);
                        }
                    }
                    if (Workflows.Count == 11)
                    {
                        if (Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
                        {
                            ListMacro.Items.Insert(Workflows.Count - 2, Workflows[^2].Content);
                            ListMacro.Items.RemoveAt(Workflows.Count - 1);
                            Workflows.RemoveAt(Workflows.Count - 1);
                        }
                    }
                }

            }
        }



        private void ExecuteWorkflowFav(WorkFlow wf, int index)
        {
            string toDelete = wf.workflowName;
            if (isEdition)
            {
                EditionWindow editionWindow = new(wf);
                editionWindow.ShowDialog();
                if (editionWindow.DialogResult == true)
                {

                    Serialization.DeleteWF(toDelete);
                    Serialization.DeleteFAV(toDelete);

                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                    serialization.Serialize(editionWindow.WorkFlow);
                    serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                    serialization.Serialize(editionWindow.WorkFlow);


                    int indexWF = 0;
                    foreach (WorkflowView wfv in Workflows)
                    {

                        if (wfv.CurrentworkFlow.workflowName.Equals(toDelete))
                        {
                            Workflows.RemoveAt(indexWF);
                            ListMacro.Items.RemoveAt(indexWF);
                            ListFav.Items.RemoveAt(index);
                            FavWorkflows.RemoveAt(index);
                            while (ListContainsName(editionWindow.WorkFlow.workflowName))
                            {
                                editionWindow.WorkFlow.workflowName += 1;
                            }

                            Workflows.Insert(indexWF, new(editionWindow.WorkFlow));
                            CreateButton(new(editionWindow.WorkFlow), false, indexWF);
                            WorkFlow favWorkflow = Workflows[indexWF].CurrentworkFlow;
                            WorkflowView favWorkflowView = new(favWorkflow);
                            FavWorkflows.Insert(index, favWorkflowView);
                            CreateButton(favWorkflowView, true, index);

                            break;
                        }
                        else
                        {
                            indexWF++;

                        }
                    }


                }




            }
            else
            {
                foreach (Block block in wf.workflowList)
                {
                    block.Execute();
                }
            }
        }


        private void ExecuteWorkflow(WorkFlow wf, int indexWF)
        {
            if (isInsearch)
            {
                indexWF = 0;

                foreach (WorkflowView wfv in Workflows)
                {
                    if (wfv.CurrentworkFlow.workflowName != wf.workflowName)
                    {
                        indexWF++;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            string toDelete = wf.workflowName;

            if (isEdition)
            {

                EditionWindow editionWindow = new(wf);
                editionWindow.ShowDialog();

                if (editionWindow.DialogResult == true)
                {
                    Serialization.DeleteWF(toDelete);

                    Workflows.RemoveAt(indexWF);
                    ListMacro.Items.RemoveAt(indexWF);
                    while (ListContainsName(editionWindow.WorkFlow.workflowName))
                    {
                        editionWindow.WorkFlow.workflowName += 1;
                    }

                    Workflows.Insert(indexWF, new(editionWindow.WorkFlow));
                    CreateButton(new(editionWindow.WorkFlow), false, indexWF);

                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                    serialization.Serialize(editionWindow.WorkFlow);



                    int indexFav = 0;
                    foreach (WorkflowView wfv in FavWorkflows)
                    {


                        if (wfv.CurrentworkFlow.workflowName.Equals(toDelete))
                        {
                            Serialization.DeleteFAV(toDelete);

                            serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                            serialization.Serialize(editionWindow.WorkFlow);


                            ListFav.Items.RemoveAt(indexFav);
                            FavWorkflows.RemoveAt(indexFav);


                            WorkflowView favWorkflowView = new(editionWindow.WorkFlow);
                            FavWorkflows.Insert(indexFav, favWorkflowView);
                            CreateButton(favWorkflowView, true, indexFav);

                            break;
                        }
                        else
                        {
                            indexFav++;

                        }
                    }
                }
            }
            else
            {
                foreach (Block block in wf.workflowList)
                {
                    block.Execute();
                }
            }
        }

        private bool ListContainsName(string workflowName)
        {
            foreach (WorkflowView wf in Workflows)
            {
                if (wf.CurrentworkFlow.workflowName.Equals(workflowName))
                {
                    return true;
                }
            }
            return false;
        }


        private void UpdateSearchList()
        {
            string searchText = Search.Text;
            WorkflowsSearchs = new();
            ListMacro.Items.Clear();
            if (!searchText.Equals(""))
            {
                isInsearch = true;
                foreach (WorkflowView workFlowView in Workflows)
                {
                    if (workFlowView.CurrentworkFlow.workflowName.ToLower().Contains(searchText.ToLower()))
                    {
                        WorkflowsSearchs.Add(workFlowView);
                        CreateButton(workFlowView, false);

                    }
                }
                if (Workflows.Count < 10)
                {
                    AddAddButton(WorkflowsSearchs);

                }


            }


        }
        private void EditionMode(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                ButtonEdit.Foreground = Brushes.Black;
                isEdition = false;
            }
            else
            {
                ButtonEdit.Foreground = Brushes.Green;
                isEdition = true;
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------//

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0:
                    ThemesController.SetTheme(ThemesController.ThemeTypes.Light);

                    break;
                case 1:
                    ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);

                    break;
            }
            e.Handled = true;
        }

    }
}
