using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MacroBoard;
namespace MacroBoard
{
    public class WorkflowView
    {
        public Button Btn_Delete { get; } = new();
        public String ImageWorkflow { get; set; } 

        public Button Btn_Fav { get; } = new();
        public Button Btn_Main { get; } = new();

        

        public Label Lbl_Name { get; } = new();
        public Grid Content { get; } = new();
        public WorkFlow CurrentworkFlow { get; set; }

        
        

        public TextBlock FavB { get; set; } = new();
        public WorkflowView(WorkFlow workFlow)
        {
            this.ImageWorkflow = workFlow.imagePath;
            this.CurrentworkFlow = workFlow;
            //Setup_Btns();
            Setup_Name(workFlow.workflowName);
            Setup_Grid();

        }

        public WorkflowView(Collection<WorkFlow> workFlows)
        {
            foreach (WorkFlow w in workFlows )
            {
            this.CurrentworkFlow = w;
            Setup_Btns();
            Setup_Name(w.workflowName);
            Setup_Grid();
            } 

        }

        public string WfName
        {
            get { return this.Lbl_Name.Content.ToString(); }
        }

       


    public void Setup_Name(string Name)
        {
            Lbl_Name.Content = Name;
            Lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
            Lbl_Name.VerticalAlignment = VerticalAlignment.Bottom;
        }


        public void Setup_Btns()
        {

            Btn_Delete.Content = FavB;
            Btn_Delete.Background = Brushes.Red;

            Btn_Delete.BorderThickness = new Thickness(0, 0, 0, 0);
            Btn_Delete.Width = 20d;
            Btn_Delete.Height = 20d;
            Btn_Delete.HorizontalAlignment = HorizontalAlignment.Left;
            Btn_Delete.VerticalAlignment = VerticalAlignment.Top;

;

            Btn_Fav.Content = FavB;
            Btn_Fav.Background = Brushes.Transparent;
            Btn_Fav.BorderThickness = new Thickness(0, 0, 0, 0);

            Btn_Fav.Width = 20d;
            Btn_Fav.Height = 20d;
            Btn_Fav.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Fav.VerticalAlignment = VerticalAlignment.Top;
        }

        public void Setup_Grid()
        {

            Content.Width = 100d;
            Content.Height = 100d;
            Btn_Fav.Content = FavB;

            Btn_Fav.Visibility = Visibility.Hidden;
            
            FavB.Visibility = Visibility.Hidden;

            Btn_Delete.Visibility = Visibility.Hidden;
            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Main);
            Content.Children.Add(Btn_Delete);
            Content.Children.Add(Btn_Fav);


            //Content.MouseEnter += OnMouseEnter;
            //Content.MouseLeave += OnMouseLeave;
        }

        //private void OnMouseEnter(object sender, RoutedEventArgs e)
        //{
        //    BitmapImage bitmapImg = new BitmapImage();

        //    bitmapImg.BeginInit();
        //    if (CurrentworkFlow.workflowName.Equals(""))
        //        bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
        //    else
        //        bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
        //    bitmapImg.EndInit();
        //    ImageWorkflow.Source = bitmapImg;

        //    Content.Background = new ImageBrush(bitmapImg);
        //}

        //private void OnMouseLeave(object sender, RoutedEventArgs e)
        //{
        //    BitmapImage bitmapImg = new BitmapImage();

        //    bitmapImg.BeginInit();
        //    if (CurrentworkFlow.workflowName.Equals(""))
        //        bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_Add.png", UriKind.Absolute);
        //    else
        //        bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
        //    bitmapImg.EndInit();
        //    ImageWorkflow.Source = bitmapImg;

        //    Content.Background = new ImageBrush(bitmapImg);
        //}


    }
}
