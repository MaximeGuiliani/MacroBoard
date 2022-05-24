using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MacroBoard.View;
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
        bool isEdition = false;

        public App CurrentApplication { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitWorkflows();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

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
            AddAddButton(Workflows);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void CreateButton(WorkflowView workflowView, bool isFav, int pos = -2)
        {
            if (isFav)
            {
                //((TextBlock)workflowView.Btn_Fav.Content).Text = "✰";


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

            if (pos != -2)
            {
                if (isFav) ListFav.Items.Insert(pos, workflowView.Content);
                else ListMacro.Items.Insert(pos, workflowView.Content);
            }
            else
            {
                if (isFav) ListFav.Items.Add(workflowView.Content);
                else ListMacro.Items.Add(workflowView.Content);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------//


        private void OnClick_Delete_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            Serialization.DeleteFAV(FavWorkflows[currentItemPos].CurrentworkFlow.workflowName);
            ListFav.Items.RemoveAt(currentItemPos);
            FavWorkflows.RemoveAt(currentItemPos);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//


        private void AddAddButton(List<WorkflowView> workflowViews)
        {
            WorkFlow addButton = new("", "", new Collection<Block>());
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
        //-----------------------------------------------------------------------------------------------------------------------------//

        private void OnClick_DeleteWorkflow(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);


            RemoveWorkflow(currentItemPos);

        }
        //-----------------------------------------------------------------------------------------------------------------------------//


        private void RemoveWorkflow(int buttonCLickedPos)
        {

            string WFToDelete = Workflows[buttonCLickedPos].CurrentworkFlow.workflowName;

            Workflows.RemoveAt(buttonCLickedPos);
            Serialization.DeleteWF(WFToDelete);



            int currentItemPosFav = 0;
            bool isInFav = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!WFToDelete.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow.workflowName))
                {
                    currentItemPosFav++;
                }
                else
                {
                    isInFav = true;
                    break;
                }
            }
            if (isInFav)
            {
                Serialization.DeleteFAV(WFToDelete);
                FavWorkflows.RemoveAt(currentItemPosFav);
                ListFav.Items.RemoveAt(currentItemPosFav);
            }
            ListMacro.Items.RemoveAt(buttonCLickedPos);



        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void OnClick_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            AddFav(new WorkflowView(Workflows[currentItemPos].CurrentworkFlow));
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void AddFav(WorkflowView newFav)
        {
            WorkFlow wf = newFav.CurrentworkFlow;
            if (FavWorkflows.Count < 5)
            {
                if (!ListContains(FavWorkflows, wf))
                {

                    CreateButton(newFav, true);

                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + wf.workflowName + ".json");
                    serialization.Serialize(wf);
                    FavWorkflows.Add(newFav);
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------//

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

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

            string searchText = Search.Text;
            List<WorkflowView> WorkflowsSearchs = new();
            if (!searchText.Equals(""))
            {
                foreach (WorkflowView workFlowView in Workflows)
                {
                    if (workFlowView.CurrentworkFlow.workflowName.ToLower().Contains(searchText.ToLower()))
                    {
                        WorkflowsSearchs.Add(workFlowView);
                    }
                }
                AddAddButton(WorkflowsSearchs);
            }
            else
            {
                WorkflowsSearchs = Workflows;
            }
            ListMacro.Items.Clear();
            foreach (WorkflowView WorkflowsSearch in WorkflowsSearchs)
            {
                ListMacro.Items.Add(WorkflowsSearch.Content);
            }

        }

        private void ResetWindow()
        {
            if (isEdition) EditionMode(new(), new());

            Search.Text = "";

            ListMacro.Items.Clear();
            foreach (WorkflowView wf in Workflows)
            {
                ListMacro.Items.Add(wf.Content);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            if (isEdition)
            {
                EditWorkflow(Workflows[currentItemPos].CurrentworkFlow, currentItemPos);
            }
            else
            {
                ExecuteWorkflow(Workflows[currentItemPos].CurrentworkFlow);

            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            if (isEdition)
            {
                EditWorkflowFav(FavWorkflows[currentItemPos].CurrentworkFlow, currentItemPos);
            }
            else
            {
                ExecuteWorkflowFav(FavWorkflows[currentItemPos].CurrentworkFlow);

            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------//

        private void AddWorkFlow(object sender, RoutedEventArgs e)
        {

            EW editionWindow = new();
            editionWindow.ShowDialog();

            ResetWindow();

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


                CreateButton(Workflows[^2], false, ListMacro.Items.Count - 1);

            }
        }

        private void EditWorkflowFav(WorkFlow wf, int index)
        {
            string toDelete = wf.workflowName;
            if (isEdition)
            {
                EW editionWindow = new(wf);
                editionWindow.ShowDialog();

                ResetWindow();

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
                            CreateButton(Workflows[indexWF], false, indexWF);
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
        }


        //-----------------------------------------------------------------------------------------------------------------------------//

        private void ExecuteWorkflowFav(WorkFlow wf)
        {
            foreach (Block block in wf.workflowList)
            {
                block.Execute();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        private void EditWorkflow(WorkFlow wf, int indexWF)
        {
            string toDelete = wf.workflowName;
            EW editionWindow = new(wf);
            editionWindow.ShowDialog();

            ResetWindow();

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
                CreateButton(Workflows[indexWF], false, indexWF);

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


        //-----------------------------------------------------------------------------------------------------------------------------//


        private static void ExecuteWorkflow(WorkFlow wf)
        {

            foreach (Block block in wf.workflowList)
            {
                block.Execute();
            }

        }
        //-----------------------------------------------------------------------------------------------------------------------------//

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




        //-----------------------------------------------------------------------------------------------------------------------------//

        private void EditionMode(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                ButtonEdit.Foreground = Brushes.Black;
                isEdition = false;
                for (int i = 0; i < ListMacro.Items.Count - 1; i++)
                {
                    ((Button)((Grid)ListMacro.Items[i]).Children[2]).Visibility = Visibility.Hidden;
                    ((Button)((Grid)ListMacro.Items[i]).Children[3]).Visibility = Visibility.Hidden;
                }



                for (int i = 0; i < ListFav.Items.Count; i++)
                {
                    ((Button)((Grid)ListFav.Items[i]).Children[3]).Visibility = Visibility.Hidden;
                }
            }
            else
            {
                ButtonEdit.Foreground = Brushes.Green;
                isEdition = true;
                for (int i = 0; i < ListMacro.Items.Count - 1; i++)
                {
                    ((Grid)ListMacro.Items[i]).MouseEnter += myRectangleLoaded;
                    ((Button)((Grid)ListMacro.Items[i]).Children[2]).Visibility = Visibility.Visible;
                    ((Button)((Grid)ListMacro.Items[i]).Children[3]).Visibility = Visibility.Visible;
                }


                for (int i = 0; i < ListFav.Items.Count; i++)
                {
                    ((Button)((Grid)ListFav.Items[i]).Children[3]).Visibility = Visibility.Visible;
                }
            }
        }


        private void myRectangleLoaded(object sender, RoutedEventArgs e)
        {

            DoubleAnimation myDoubleAnimation = new();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = 360;


            Storyboard.SetTargetProperty(myDoubleAnimation,
                new PropertyPath(RotateTransform.AngleProperty));
            Storyboard myStoryboard = new();
            myStoryboard.Children.Add(myDoubleAnimation);
            myStoryboard.Begin(this);
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
