using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public MainWindow()
        {
            InitializeComponent();
            InitWorkflows();
        }
        private void InitWorkflows()
        {
            getWorkFlowsFromJson();
            getFavsFromJson();

            foreach (WorkflowView FavWorkflow in FavWorkflows)
            {
                FavWorkflow.Btn_Delete.Visibility = Visibility.Hidden;
                FavWorkflow.Btn_Main.Click += Button_Click_Fav;
                FavWorkflow.Btn_Fav.Click += OnClick_Delete_Fav;
                if (!FavWorkflow.CurrentworkFlow.imagePath.Equals(""))
                {
                    FavWorkflow.Btn_Main.Content =
                        new Image
                        {
                            Source = new BitmapImage(new Uri(FavWorkflow.CurrentworkFlow.imagePath, UriKind.Absolute))
                        };
                }
                ListFav.Items.Add(FavWorkflow.Content);
            }
            foreach (WorkflowView workflowView in Workflows)
            {
                workflowView.Btn_Delete.Click += OnClick_DeleteWorkflow;
                workflowView.Btn_Fav.Click += OnClick_Fav;
                workflowView.Btn_Main.Click += Button_Click;
                if (!workflowView.CurrentworkFlow.imagePath.Equals(""))
                {
                    workflowView.Btn_Main.Content = new Image
                    {
                        Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.Absolute))
                    };
                }
                ListMacro.Items.Add(workflowView.Content);
            }

            //add buton initialization
            AddAddButton(Workflows);
        }

        private void AddWorkFlowWhileSearch(object sender, RoutedEventArgs e)
        {
            EditionWindow editionWindow = new();
            editionWindow.ShowDialog();

            WorkFlow wf = editionWindow.WorkFlow;
            // WorkFlow macroAddTest = new("", "Test6", new List<Block>());

            //AddWorkFLowToJSON(editionWindow.workFlow);

            Workflows.Insert(Workflows.Count - 1, new(wf));
            Workflows[^2].Btn_Delete.Click += OnClick_DeleteWorkflow;
            Workflows[^2].Btn_Fav.Click += OnClick_Fav;
            Workflows[^2].Btn_Main.Click += Button_Click;
            Workflows[^2].Btn_Main.Content = new Image
            {
                Source = new BitmapImage(new Uri(wf.imagePath, UriKind.Absolute))
            };
            if (Workflows.Count == 11 && Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
            {
                Workflows.RemoveAt(Workflows.Count - 1);
            }
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
                WorkFlow wf = editionWindow.WorkFlow;
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

            if (!isInsearch)
            {
                RemoveWorkflow(currentItemPos);
            }
            else
            {

                RemoveWorkflowWhileSearch(WorkflowsSearchs, currentItemPos);
                WorkflowsSearchs.RemoveAt(currentItemPos);
            }

            if (Workflows.Count == 9 && !Workflows[^1].CurrentworkFlow.workflowName.Equals(""))
            {
                AddAddButton(Workflows);
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
                                Source = new BitmapImage(new Uri("../../../Resources/add.png", UriKind.Relative))
                            };
            ListMacro.Items.Add(workflowViews[^1].Content);
        }

        private void RemoveWorkflowWhileSearch(List<WorkflowView> wfs, int buttonClickedPos)
        {
            int currentItemPosFav = 0;
            bool test = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!wfs[buttonClickedPos].CurrentworkFlow.workflowName.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow.workflowName))
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
            int currentItemPos = 0;
            while (currentItemPos < Workflows.Count)
            {
                if (!wfs[buttonClickedPos].CurrentworkFlow.Equals(Workflows[currentItemPos].CurrentworkFlow))
                {
                    currentItemPos++;
                }
                else
                {
                    break;
                }
            }
            Serialization.DeleteWF(Workflows[currentItemPos].CurrentworkFlow.workflowName);
            Workflows.RemoveAt(currentItemPos);
            ListMacro.Items.RemoveAt(buttonClickedPos);
        }
        private void RemoveWorkflow(int buttonCLickedPos)
        {
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
            ListMacro.Items.RemoveAt(buttonCLickedPos);
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
                ExecuteWorkflow((WorkflowsSearchs[currentItemPos].CurrentworkFlow));
            }
            else
            {
                ExecuteWorkflow((Workflows[currentItemPos].CurrentworkFlow));
            }
        }
        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            ExecuteWorkflow(FavWorkflows[currentItemPos].CurrentworkFlow);
        }

        private void ExecuteWorkflow(WorkFlow wf)
        {
            if (isEdition)
            {
                EditionWindow editionWindow = new(wf);
                editionWindow.ShowDialog();

            }
            else
            {
                foreach (Block block in wf.workflowList)
                {
                    block.Execute();
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


        private void getFavsFromJson()
        {
            int fCount = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON", "*", SearchOption.TopDirectoryOnly).Length;
            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON");
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + file.Name);
                FavWorkflows.Add(new(serialization.Deserialize()));
            }
        }

        private void getWorkFlowsFromJson()
        {
            int fCount = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON", "*", SearchOption.TopDirectoryOnly).Length;
            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON");
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + file.Name);
                Workflows.Add(new(serialization.Deserialize()));
            }
        }
    }



}
