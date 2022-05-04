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
        private List<WorkflowView> WorkflowsSearch = new();
        bool isEdition = false;
        bool isInsearch = false;
        WorkFlow macro0 = new("", "Test0", new List<Block>());
        WorkFlow macro1 = new("", "Test1", new List<Block>());
        WorkFlow macro2 = new("", "Test2", new List<Block>());
        WorkFlow macro3 = new("", "Test3", new List<Block>());
        WorkFlow macro4 = new("", "Test4", new List<Block>());
        WorkFlow macro5 = new("", "Test5", new List<Block>());



        public MainWindow()
        {
            InitializeComponent();
            InitBlock();
        }

        private void InitBlock()
        {
            FavWorkflows.Add(new(macro0));
            FavWorkflows.Add(new(macro1));
            FavWorkflows.Add(new(macro2));
            FavWorkflows.Add(new(macro3));
            Workflows.Add(new(macro0));
            Workflows.Add(new(macro1));
            Workflows.Add(new(macro2));
            Workflows.Add(new(macro3));
            Workflows.Add(new(macro4));
            Workflows.Add(new(macro5));


            foreach (WorkflowView FavWorkflow in FavWorkflows)
            {
                FavWorkflow.Btn_Delete.Click += OnClick_Delete_Fav;
                FavWorkflow.Btn_Main.Click += Button_Click_Fav;
                FavWorkflow.Btn_Fav.Visibility = Visibility.Hidden;
                ListFav.Items.Add(FavWorkflow.Content);
            }
            foreach (WorkflowView Workflow in Workflows)
            {
                Workflow.Btn_Delete.Click += OnClick_Delete;
                Workflow.Btn_Fav.Click += OnClick_Fav;
                Workflow.Btn_Main.Click += Button_Click;
                ListMacro.Items.Add(Workflow.Content);
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
                removeWorkflow(Workflows, currentItemPos);
            }
            else
            {
                removeWorkflow(WorkflowsSearch, currentItemPos);
                WorkflowsSearch.RemoveAt(currentItemPos);
            }

        }





        private void removeWorkflow(List<WorkflowView> wfs, int index)
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

            ListMacro.Items.RemoveAt(index);
            Workflows.RemoveAt(index);

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
                AddFav(new WorkflowView(WorkflowsSearch[currentItemPos].CurrentworkFlow));
            }

        }

        private void AddFav(WorkflowView newFav)
        {
            newFav.Btn_Delete.Click += OnClick_Delete_Fav;
            newFav.Btn_Main.Click += Button_Click_Fav;
            newFav.Btn_Fav.Visibility = Visibility.Hidden;
            ListFav.Items.Add(newFav.Content);
            FavWorkflows.Add(newFav);

        }


        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = Search.Text;
            WorkflowsSearch = new();
            if (txt != "")
            {
                isInsearch = true;
                foreach (WorkflowView m in Workflows)
                {
                    if (m.CurrentworkFlow.workflowName.Contains(txt))
                        WorkflowsSearch.Add(m);
                }
            }
            else
            {
                isInsearch = false;
                WorkflowsSearch = Workflows;
            }
            ListMacro.Items.Clear();
            foreach (WorkflowView mac in WorkflowsSearch)
            {
                ListMacro.Items.Add(mac.Content);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            if (isEdition)
            {
                EditionWindow editionWindow = new(Workflows[currentItemPos].CurrentworkFlow);
                editionWindow.Show();
            }
            else
            {
                foreach (Block m in Workflows[currentItemPos].CurrentworkFlow.workflowList)
                {
                    m.Execute();
                }

            }
        }
        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);

            if (isEdition)
            {

                new EditionWindow();
            }
            else
            {
                foreach (Block m in FavWorkflows[currentItemPos].CurrentworkFlow.workflowList)
                {
                    m.Execute();
                }

            }
        }

        private void EditionMode(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                ButtonEdit.Background = Brushes.Green;
                isEdition = false;
            }
            else
            {
                ButtonEdit.Background = Brushes.Red;
                isEdition = true;
            }
        }
    }

}
