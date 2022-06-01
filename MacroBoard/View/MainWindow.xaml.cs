using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;
using static MacroBoard.Utils;
using System.Linq;
using System.Windows.Media.Animation;
using MacroBoard.View;
using MacroBoard.View.Themes;
using System.Windows.Data;
using System.Collections.Generic;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WorkFlow WorkFlow;       
        public ObservableCollection<WorkflowView> FavoriteWorkFlows { get; set; }
        public ObservableCollection<WorkflowView> WorkFlows { get; set; }

        bool isEdition = false;

        public MainWindow()
        {
            InitializeComponent();
            InitWorkflows();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void InitWorkflows()
        {
            this.DataContext = this;

            WorkFlows = new ();
            FavoriteWorkFlows = new();
            
            WorkFlows = Serialization.getWorkFlowsFromJsonZ();
            FavoriteWorkFlows = Serialization.getFavsFromJsonZ();

            foreach (WorkflowView workflowView in WorkFlows)
            {
                CreateButton(workflowView, false);
            }
            foreach (WorkflowView FavWorkflows in FavoriteWorkFlows)
            {
                CreateButton(FavWorkflows, true);
            }
           
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void CreateButton(WorkflowView workflowView, bool isFav, int pos = -2)
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
                workflowView.Btn_Fav.Click += OnClick_Favorite;
                workflowView.Btn_Main.Click += Button_Click;
            }

            if (!workflowView.CurrentworkFlow.imagePath.Equals(""))
            {
                //ImageWorkflow
                Image image = new() {Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.RelativeOrAbsolute)) };

                image.Stretch = Stretch.Fill;   
                workflowView.Btn_Main.Content = image;
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
            Serialization.DeleteFAV(FavoriteWorkFlows[currentItemPos].CurrentworkFlow.workflowName);
            ListFav.Items.RemoveAt(currentItemPos);
            FavoriteWorkFlows.RemoveAt(currentItemPos);
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

            string WFToDelete = WorkFlows[buttonCLickedPos].CurrentworkFlow.workflowName;

            WorkFlows.RemoveAt(buttonCLickedPos);
            Serialization.DeleteWF(WFToDelete);



            int currentItemPosFav = 0;
            bool isInFav = false;
            while (currentItemPosFav < FavoriteWorkFlows.Count)
            {
                if (!WFToDelete.Equals(FavoriteWorkFlows[currentItemPosFav].CurrentworkFlow.workflowName))
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
                FavoriteWorkFlows.RemoveAt(currentItemPosFav);
                ListFav.Items.RemoveAt(currentItemPosFav);
            }
            ListMacro.Items.RemoveAt(buttonCLickedPos);



        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void OnClick_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            AddFav(new WorkflowView(WorkFlows[currentItemPos].CurrentworkFlow));
        }
        ////////////////////////////////////////////////////////////////////////
        private void OnClick_Favorite(object sender, RoutedEventArgs e)
        {

            int currentItemPos = ListMacro.Items.IndexOf(((Button)sender).Parent);
            AddFavorite(new WorkflowView(WorkFlows[currentItemPos].CurrentworkFlow));
        }

        private void AddFavorite(WorkflowView newFavorite)
        {
            WorkFlow wf = newFavorite.CurrentworkFlow;
            if (FavoriteWorkFlows.Count < 5)
            {
                {
                    CreateButton(newFavorite, true);
                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + wf.workflowName + ".json");
                    serialization.Serialize(wf);
                    FavoriteWorkFlows.Add(newFavorite);
                }
            }
        }



        //-----------------------------------------------------------------------------------------------------------------------------//

        private void AddFav(WorkflowView newFav)
        {
            WorkFlow wf = newFav.CurrentworkFlow;
            if (FavoriteWorkFlows.Count < 5)
            {
                {

                    CreateButton(newFav, true);

                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + wf.workflowName + ".json");
                    serialization.Serialize(wf);
                    FavoriteWorkFlows.Add(newFav);
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

        private void Search_TextChanged(object sender, RoutedEventArgs e)
        {

            string searchText = Search.Text;
            ObservableCollection<WorkflowView> WorkflowsSearchs = new();
            if (!searchText.Equals(""))
            {
                foreach (WorkflowView workFlowView in WorkFlows)
                {
                    if (workFlowView.CurrentworkFlow.workflowName.ToLower().Contains(searchText.ToLower()))
                    {
                        WorkflowsSearchs.Add(workFlowView);
                    }
                }
                
            }
            else
            {
                WorkflowsSearchs = WorkFlows;
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
            foreach (WorkflowView wf in WorkFlows)
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
                EditWorkflow(WorkFlows[currentItemPos].CurrentworkFlow, currentItemPos);
            }
            else
            {
                ExecuteWorkflow(WorkFlows[currentItemPos].CurrentworkFlow) ;

            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListFav.Items.IndexOf(((Button)sender).Parent);
            if (isEdition)
            {
                EditWorkflowFav(FavoriteWorkFlows[currentItemPos].CurrentworkFlow, currentItemPos);
            }
            else
            {
                ExecuteWorkflowFav(FavoriteWorkFlows[currentItemPos].CurrentworkFlow);

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


                WorkFlows.Insert(WorkFlows.Count - 1, new(wf));


                CreateButton(WorkFlows[^2], false, ListMacro.Items.Count - 1);

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
                    foreach (WorkflowView wfv in WorkFlows)
                    {

                        if (wfv.CurrentworkFlow.workflowName.Equals(toDelete))
                        {
                            WorkFlows.RemoveAt(indexWF);
                            ListMacro.Items.RemoveAt(indexWF);
                            ListFav.Items.RemoveAt(index);
                            FavoriteWorkFlows.RemoveAt(index);
                            while (ListContainsName(editionWindow.WorkFlow.workflowName))
                            {
                                editionWindow.WorkFlow.workflowName += 1;
                            }

                            WorkFlows.Insert(indexWF, new(editionWindow.WorkFlow));
                            CreateButton(WorkFlows[indexWF], false, indexWF);
                            WorkFlow favWorkflow = WorkFlows[indexWF].CurrentworkFlow;
                            WorkflowView favWorkflowView = new(favWorkflow);
                            FavoriteWorkFlows.Insert(index, favWorkflowView);
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

                WorkFlows.RemoveAt(indexWF);
                ListMacro.Items.RemoveAt(indexWF);
                while (ListContainsName(editionWindow.WorkFlow.workflowName))
                {
                    editionWindow.WorkFlow.workflowName += 1;
                }

                WorkFlows.Insert(indexWF, new(editionWindow.WorkFlow));
                CreateButton(WorkFlows[indexWF], false, indexWF);

                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                serialization.Serialize(editionWindow.WorkFlow);
                int indexFav = 0;
                foreach (WorkflowView wfv in FavoriteWorkFlows)
                {
                    if (wfv.CurrentworkFlow.workflowName.Equals(toDelete))
                    {
                        Serialization.DeleteFAV(toDelete);
                        serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + editionWindow.WorkFlow.workflowName + ".json");
                        serialization.Serialize(editionWindow.WorkFlow);
                        ListFav.Items.RemoveAt(indexFav);
                        FavoriteWorkFlows.RemoveAt(indexFav);
                        WorkflowView favWorkflowView = new(editionWindow.WorkFlow);
                        FavoriteWorkFlows.Insert(indexFav, favWorkflowView);
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
            foreach (WorkflowView wf in WorkFlows)
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

        bool isDark = true;
        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (isDark)
                ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
            else
                ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);

            isDark = !isDark;
        }

        private void AboutApp(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("about");
        }

    }

       
}
