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
using MacroBoard.Model;
using MacroBoard.View;
using MacroBoard.View.Themes;
using System.Windows.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MacroBoard

{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
        public WorkFlow WorkFlow;       
        public ObservableCollection<WorkflowView> FavoriteWorkFlows { get; set; }
        public ObservableCollection<WorkflowView> WorkFlows { get; set; }

        public Observable<bool> isEdition { get; set; } = new(false);

        myTcpListener Server;



        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitWorkflows();
        }


        public MainWindow(string message) : this()
        {
            //new myTcpListener();
            InitializeComponent();
            InitWorkflows();
            MessageBox.Show(message);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//



        private void InitWorkflows()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            this.DataContext = this;

            WorkFlows = new ();
            FavoriteWorkFlows = new();
           

            WorkFlows = Serialization.getWorkFlowsFromJsonZ();
            FavoriteWorkFlows = Serialization.getFavsFromJsonZ();

           
            ListMacro.ItemsSource = WorkFlows;
            ListFav.ItemsSource = FavoriteWorkFlows;



        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        private void cbFeature_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (AppServer.IsChecked == true)
            {
                Server = new myTcpListener();
            }
            else
            {
                if (Server.isDatasender)
                {
                    Server.server.Stop();

                }
                else
                {
                    Server.dataReceiveServer.Stop();

       }
            }
        }

        private void CreateButton(WorkflowView workflowView, bool isFav, int pos = -2)
        {

            if (isFav)
            {
                if (isEdition.Value)
                {
                    workflowView.Btn_Fav.Visibility = Visibility.Visible;

                }
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
                Image image = new() { Source = new BitmapImage(new Uri(workflowView.CurrentworkFlow.imagePath, UriKind.RelativeOrAbsolute)) };
                image.Stretch = Stretch.Fill;
                workflowView.Btn_Main.Content = image;


            }
            
            if (pos != -2)
            {
                if (isFav) FavoriteWorkFlows[pos] = workflowView;
                else WorkFlows[pos] = workflowView;
            }

           
        }

        //-----------------------------------------------------------------------------------------------------------------------------//


        private void OnClick_Delete_Fav(object sender, RoutedEventArgs e)
        {
            WorkflowView wf = (WorkflowView)((Button)sender).DataContext;
            int currentItemPos = FavoriteWorkFlows.IndexOf(wf);
            Serialization.DeleteFAV(FavoriteWorkFlows[currentItemPos].CurrentworkFlow.workflowName);
            FavoriteWorkFlows.RemoveAt(currentItemPos);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void OnClick_DeleteWorkflow(object sender, RoutedEventArgs e)
        {
            WorkflowView wf = (WorkflowView)((Button)sender).DataContext;
            int currentItemPos = WorkFlows.IndexOf(wf);
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
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------//

        private void OnClick_Favorite(object sender, RoutedEventArgs e)
        {
            WorkflowView wf = (WorkflowView)((Button)sender).DataContext;
            int currentItemPos = WorkFlows.IndexOf(wf);
            
            AddFavorite(new WorkflowView(WorkFlows[currentItemPos].CurrentworkFlow));
           
        }

        private void AddFavorite(WorkflowView newFavorite)
        {
            WorkFlow wf = newFavorite.CurrentworkFlow;
            if (FavoriteWorkFlows.Count < 5)
            {
                if (!ListContains(FavoriteWorkFlows, wf))
                {
                    CreateButton(newFavorite, true);
                    Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + wf.workflowName + ".json");
                    serialization.Serialize(wf);
                    FavoriteWorkFlows.Add(newFavorite);
                }
            }
        }



        //-----------------------------------------------------------------------------------------------------------------------------//

        private static bool ListContains(ObservableCollection<WorkflowView> workflowViews, WorkFlow workFlow)
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
                ListMacro.ItemsSource = WorkFlows;
            }

            ListMacro.ItemsSource = WorkflowsSearchs;
           



        }

        private void ResetWindow()
        {
            if (isEdition.Value) EditionMode(new(), new());

            Search.Text = "";

            WorkFlows.Clear();


            ListMacro.ItemsSource = WorkFlows;

        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkflowView wf = (WorkflowView)((Border)sender).DataContext;
            int currentItemPos = WorkFlows.IndexOf(wf);
            if (isEdition.Value)
            {
                EditWorkflow(WorkFlows[currentItemPos].CurrentworkFlow, currentItemPos);
            }
            else
            {
                ExecuteWorkflow(WorkFlows[currentItemPos].CurrentworkFlow);

            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        private void Button_Click_Fav(object sender, RoutedEventArgs e)
        {
            WorkflowView wf = (WorkflowView)((Border)sender).DataContext;
            int currentItemPos = FavoriteWorkFlows.IndexOf(wf);

            if (isEdition.Value)
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

                WorkflowView workflowView = new WorkflowView(wf);


                WorkFlows.Add(workflowView);
                
                CreateButton(workflowView, false, WorkFlows.Count - 1 );


            }
            ListMacro.ItemsSource = WorkFlows;

        }



        private void EditWorkflowFav(WorkFlow wf, int index)
        {
            string toDelete = wf.workflowName;
            if (isEdition.Value)
            {
                EditionWindow editionWindow = new(wf);
                editionWindow.ShowDialog();

                //ResetWindow();

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
            Executor executor = new(wf);
            string message = executor.Execute();
            if (message.Length != 0)
                MessageBox.Show(message);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        private void EditWorkflow(WorkFlow wf, int indexWF)
        {
            string toDelete = wf.workflowName;
            EditionWindow editionWindow = new(wf);
            editionWindow.ShowDialog();

            //ResetWindow();

            if (editionWindow.DialogResult == true)
            {
                Serialization.DeleteWF(toDelete);

                WorkFlows.RemoveAt(indexWF);
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
            Executor executor = new(wf);
            string message = executor.Execute();
            if (message.Length != 0)
                MessageBox.Show(message);

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
            isEdition.Value = !isEdition.Value;


            //if (isEdition)
            //{
            //    isEdition = false;

            //    for (int i = 0; i < WorkFlows.Count - 1; i++)
            //    {
                    
            //        WorkFlows[i].Btn_Fav.Visibility = Visibility.Hidden;
            //        WorkFlows[i].Btn_Delete.Visibility = Visibility.Hidden;
            //    }

            //    for (int i = 0; i < FavoriteWorkFlows.Count; i++)
            //    {
                    
            //        FavoriteWorkFlows[i].Btn_Fav.Visibility = Visibility.Hidden;
            //    }
            //}
            //else
            //{
            //    isEdition = true;

            //    for (int i = 0; i < WorkFlows.Count ; i++)

            //    {
            //        WorkFlows[i].Content.MouseEnter += myRectangleLoaded;
            //        WorkFlows[i].Btn_Fav.Visibility = Visibility.Visible;
            //        WorkFlows[i].Btn_Delete.Visibility = Visibility.Visible;
            //    }


            //    for (int i = 0; i < FavoriteWorkFlows.Count ; i++)
            //    {
            //        FavoriteWorkFlows[i].Btn_Delete.Visibility = Visibility.Visible;
            //    }
            //}

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

        public event PropertyChangedEventHandler? PropertyChanged;


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

            About about = new();
            about.ShowDialog();
        }


        //---------------------------------------------------------------------------------------



        public class Observable<T> : INotifyPropertyChanged
        {
            public Observable(T initialValue)
            {
                this.Value = initialValue;
            }

            private T _value;
            public T Value
            {
                get { return _value; }
                set { _value = value; NotifyPropertyChanged("Value"); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            internal void NotifyPropertyChanged(String propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }












    }
}
