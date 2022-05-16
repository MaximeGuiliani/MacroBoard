using System;
using System.Collections.Generic;
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
            List<Block> macroNotePads = new();
            macroNotePads.Add(new BlockRunApp("notepad.exe"));
            macroNotePads.Add(new BlockWait(0, 2, 0, 0));
            macroNotePads.Add(new BlockKeyBoard("hello world ^s "));
            WorkFlow macroNotePad = new("", "macroNotePads", macroNotePads);

            List<Block> machromes = new();
            machromes.Add(new BlockLaunchBrowserChromex86("https://royaleapi.com/player/2GPUV2Y0"));
            machromes.Add(new BlockWait(0, 2, 0, 0));
            machromes.Add(new BlockScreenshot($@"C:\Users\maxim\OneDrive\Bureau\test.png", 0));
            WorkFlow machrome = new("", "machrome", machromes);

            List<Block> mailcros = new();
            mailcros.Add(new BlockSendEmail("test", "lpmusardo@gmail.com", "Subject"));
            mailcros.Add(new BlockWait(0, 2, 0, 0));
            mailcros.Add(new BlockRecognition($@"C:\Users\maxim\OneDrive\Bureau\gmail.png", loop: true, debugMode: true));
            mailcros.Add(new BlockClickL());
            mailcros.Add(new BlockWait(0, 2, 0, 0));
            mailcros.Add(new BlockRecognition($@"C:\Users\maxim\OneDrive\Bureau\send.jpeg", loop: true));
            mailcros.Add(new BlockClickL());

            WorkFlow mailcro = new("", "mailcro", mailcros);
            WorkFlow lpmacro = new("", "lpmacro", new List<Block>());
            WorkFlow macro4 = new("", "Test4", new List<Block>());
            WorkFlow macro5 = new("", "Test5", new List<Block>());
            FavWorkflows.Add(new(macroNotePad));
            FavWorkflows.Add(new(machrome));
            FavWorkflows.Add(new(mailcro));
            FavWorkflows.Add(new(lpmacro));
            Workflows.Add(new(macroNotePad));
            Workflows.Add(new(machrome));
            Workflows.Add(new(mailcro));
            Workflows.Add(new(lpmacro));
            Workflows.Add(new(macro4));
            Workflows.Add(new(macro5));

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
                            Source = new BitmapImage(new Uri(FavWorkflow.CurrentworkFlow.imagePath, UriKind.Relative))
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
                        Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.Relative))
                    };
                }
                ListMacro.Items.Add(workflowView.Content);
            }
            //add buton initialization
            AddAddButton(Workflows);
        }

        private void AddWorkFLowToJSON(WorkFlow workFlow)
        {

        }

        private void AddWorkFlowWhileSearch(object sender, RoutedEventArgs e)
        {
            EditionWindow editionWindow = new();
            editionWindow.Show();
            WorkFlow macroAddTest = new("", "Test6", new List<Block>());

            //AddWorkFLowToJSON(editionWindow.workFlow);

            Workflows.Insert(Workflows.Count - 1, new(macroAddTest));
            Workflows[^2].Btn_Delete.Click += OnClick_DeleteWorkflow;
            Workflows[^2].Btn_Fav.Click += OnClick_Fav;
            Workflows[^2].Btn_Main.Click += Button_Click;

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
                editionWindow.Show();
                WorkFlow macroAddTest = new("", "Test6", new List<Block>());
                Workflows.Insert(Workflows.Count - 1, new(macroAddTest));
                Workflows[^2].Btn_Delete.Click += OnClick_DeleteWorkflow;
                Workflows[^2].Btn_Fav.Click += OnClick_Fav;
                Workflows[^2].Btn_Main.Click += Button_Click;
                if (!Workflows[^2].CurrentworkFlow.imagePath.Equals(""))
                {
                    Workflows[^2].Btn_Main.Content
                 = new Image
                 {
                     Source = new BitmapImage(new Uri(Workflows[^2].CurrentworkFlow.imagePath, UriKind.Relative))
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

        private void InitWorkflows(List<WorkFlow> workflows)
        {

        }
        private void InitFavWorkflows(List<WorkFlow> workflows)
        {

        }

        private void OnClick_Delete_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            ListFav.Items.RemoveAt(currentItemPos);
            FavWorkflows.RemoveAt(currentItemPos);
        }


        private void OnClick_DeleteWorkflow(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);

            if (!isInsearch)
            {
                RemoveWorkflow(Workflows, currentItemPos);
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
                if (!wfs[buttonClickedPos].CurrentworkFlow.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow))
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
            Workflows.RemoveAt(currentItemPos);
            ListMacro.Items.RemoveAt(buttonClickedPos);
        }
        private void RemoveWorkflow(List<WorkflowView> wfs, int buttonCLickedPos)
        {
            int currentItemPosFav = 0;
            bool test = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!wfs[buttonCLickedPos].CurrentworkFlow.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow))
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
                FavWorkflows.RemoveAt(currentItemPosFav);
                ListFav.Items.RemoveAt(currentItemPosFav);
            }
            ListMacro.Items.RemoveAt(currentItemPosFav);
            Workflows.RemoveAt(currentItemPosFav);
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
                                Source = new BitmapImage(new Uri(newFav.CurrentworkFlow.imagePath, UriKind.Relative))
                            };
                    }

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
                if (workflowView.CurrentworkFlow.Equals(workFlow))
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
                editionWindow.Show();
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
