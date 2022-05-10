using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            List<Block> macroNotePads = new();
            macroNotePads.Add(new BlockRunApp("notepad.exe"));
            macroNotePads.Add(new BlockWait(0, 0, 2));
            macroNotePads.Add(new BlockKeyBoard("hello world ^s "));
            WorkFlow macroNotePad = new("", "macroNotePads", macroNotePads);
            List<Block> machromes = new();
            machromes.Add(new BlockLaunchBrowserChromex86("https://royaleapi.com/player/2GPUV2Y0"));
            machromes.Add(new BlockWait(0, 0, 2));
            machromes.Add(new BlockScreenshot($@"C:\Users\maxim\OneDrive\Bureau\test.png", 0));
            WorkFlow machrome = new("", "machrome", machromes);
            List<Block> mailcro = new();
            mailcro.Add(new BlockSendEmail("test", "lpmusardo@gmail.com", "Subject"));
            mailcro.Add(new BlockWait(0, 0, 2));
            mailcro.Add(new BlockRecognition($@"C:\Users\maxim\OneDrive\Bureau\gmail.png", debugMode: true));
            mailcro.Add(new BlockClickL());
            mailcro.Add(new BlockWait(0, 0, 2));
            mailcro.Add(new BlockRecognition($@"C:\Users\maxim\OneDrive\Bureau\send.jpeg"));
            mailcro.Add(new BlockClickL());
            WorkFlow macro2 = new("", "mailcro", mailcro);
            WorkFlow lpmacro = new("", "lpmacro", new List<Block>());
            WorkFlow macro4 = new("", "Test4", new List<Block>());
            WorkFlow macro5 = new("", "Test5", new List<Block>());
            FavWorkflows.Add(new(macroNotePad));
            FavWorkflows.Add(new(machrome));
            FavWorkflows.Add(new(macro2));
            FavWorkflows.Add(new(lpmacro));
            Workflows.Add(new(macroNotePad));
            Workflows.Add(new(machrome));
            Workflows.Add(new(macro2));
            Workflows.Add(new(lpmacro));
            Workflows.Add(new(macro4));
            Workflows.Add(new(macro5));
            foreach (WorkflowView FavWorkflow in FavWorkflows)
            {
                FavWorkflow.Btn_Delete.Visibility = Visibility.Hidden;
                FavWorkflow.Btn_Main.Click += Button_Click_Fav;
                FavWorkflow.Btn_Fav.Click += OnClick_Delete_Fav;
                ListFav.Items.Add(FavWorkflow.Content);
            }
            foreach (WorkflowView Workflow in Workflows)
            {
                Workflow.Btn_Delete.Click += OnClick_Delete;
                Workflow.Btn_Fav.Click += OnClick_Fav;
                Workflow.Btn_Main.Click += Button_Click;
                ListMacro.Items.Add(Workflow.Content);
            }
            //add buton initialization
            AddAddButton(Workflows);
        }
        private void AddWorkFlowWhileSearch(object sender, RoutedEventArgs e)
        {
            EditionWindow editW = new();
            editW.ShowDialog();
            WorkFlow macroAddTest = new("", "Test6", new List<Block>());
            Workflows.Insert(Workflows.Count - 1, new(macroAddTest));
            Workflows[^2].Btn_Delete.Click += OnClick_Delete;
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


                EditionWindow editW = new();
                editW.ShowDialog();
                WorkFlow macroAddTest = new("", "Test6", new List<Block>());
                Workflows.Insert(Workflows.Count - 1, new(macroAddTest));
                Workflows[^2].Btn_Delete.Click += OnClick_Delete;
                Workflows[^2].Btn_Fav.Click += OnClick_Fav;
                Workflows[^2].Btn_Main.Click += Button_Click;
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
            ListFav.Items.RemoveAt(currentItemPos);
            FavWorkflows.RemoveAt(currentItemPos);
        }


        private void OnClick_Delete(object sender, RoutedEventArgs e)
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
            WorkFlow addButton = new("", "", null);
            workflowViews.Add(new(addButton));
            workflowViews[^1].Btn_Delete.Visibility = Visibility.Hidden;

            workflowViews[^1].Btn_Fav.Visibility = Visibility.Hidden;
            workflowViews[^1].Btn_Main.Click += AddWorkFlow;
            BitmapImage bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_Add.png", UriKind.Absolute);
            bitmapImg.EndInit();
            workflowViews[^1].Content.Background = new ImageBrush(bitmapImg);
            ListMacro.Items.Add(workflowViews[^1].Content);
        }

        private void RemoveWorkflowWhileSearch(List<WorkflowView> wfs, int index)
        {
            int currentItemPosFav = 0;
            bool test = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!wfs[index].CurrentworkFlow.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow))
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
                if (!wfs[index].CurrentworkFlow.Equals(Workflows[currentItemPos].CurrentworkFlow))
                {
                    currentItemPos++;
                }
                else
                {
                    break;
                }
            }
            Workflows.RemoveAt(currentItemPos);
            ListMacro.Items.RemoveAt(index);
        }
        private void RemoveWorkflow(List<WorkflowView> wfs, int index)
        {
            int currentItemPosFav = 0;
            bool test = false;
            while (currentItemPosFav < FavWorkflows.Count)
            {
                if (!wfs[index].CurrentworkFlow.Equals(FavWorkflows[currentItemPosFav].CurrentworkFlow))
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
                    ListFav.Items.Add(newFav.Content);
                    FavWorkflows.Add(newFav);
                }
            }
        }

        private bool ListContains(List<WorkflowView> wfls, WorkFlow wf)
        {
            bool result = false;
            foreach (WorkflowView wfItem in wfls)
            {
                if (wfItem.CurrentworkFlow.Equals(wf))
                {
                    return true;
                }
            }
            return result;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = Search.Text;
            WorkflowsSearchs = new();
            if (!txt.Equals(""))
            {
                isInsearch = true;
                foreach (WorkflowView m in Workflows)
                {
                    if (m.CurrentworkFlow.workflowName.ToLower().Contains(txt.ToLower()))
                    {
                        WorkflowsSearchs.Add(m);

                    }
                }

                AddAddButton(WorkflowsSearchs);

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
                foreach (Block m in wf.workflowList)
                {
                    m.Execute();
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
    }
}
