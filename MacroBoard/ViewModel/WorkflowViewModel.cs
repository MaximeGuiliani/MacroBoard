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
        public Image ImageWorkflow { get; } = new();

        public Button Btn_Fav { get; } = new();
        public Button Btn_Main { get; } = new();
        

        private Label Lbl_Name = new();
        public Grid Content { get; } = new();
        public WorkFlow CurrentworkFlow { get; set; }

        public WorkflowView(WorkFlow workFlow)
        {

            this.CurrentworkFlow = workFlow;
            Setup_Btns();
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


        public void Setup_Name(string Name)
        {
            Lbl_Name.Content = Name;
            Lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
            Lbl_Name.VerticalAlignment = VerticalAlignment.Bottom;
        }


        public void Setup_Btns()
        {

            //Main Button
            //Btn_Main.Background = Brushes.LightGray;
            Btn_Main.Opacity = 0.8;
            
            Btn_Main.Width = 90d;
            Btn_Main.Height = 90d;

            Btn_Main.Resources.Add(Border.CornerRadiusProperty, new CornerRadius(15)) ;
            Btn_Main.Resources.Add(Border.BorderBrushProperty, Brushes.Red) ;
            Btn_Main.Resources.Add(Border.BorderThicknessProperty, new Thickness(10)) ;
            //Delete Button
           TextBlock txtBlock = new();

            txtBlock.Text = "━";
            txtBlock.Foreground = Brushes.Red;
            txtBlock.FontSize = 20d;


            Btn_Delete.Content = txtBlock;
            Btn_Delete.Background = Brushes.Transparent;
            Btn_Delete.BorderThickness = new Thickness(0, 0, 0, 0);
            Btn_Delete.Width = 20d;
            Btn_Delete.Height = 20d;
            Btn_Delete.HorizontalAlignment = HorizontalAlignment.Left;
            Btn_Delete.VerticalAlignment = VerticalAlignment.Top;


            //Edit Button
            txtBlock = new();
            txtBlock.Text = "♥";
            txtBlock.Foreground = Brushes.Red;
            txtBlock.FontSize = 20d;

            Btn_Fav.Content = txtBlock;
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

            Btn_Fav.Visibility = Visibility.Hidden;

            Btn_Delete.Visibility = Visibility.Hidden;
            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Main);
            Content.Children.Add(Btn_Delete);
            Content.Children.Add(Btn_Fav);


            //Content.MouseEnter += OnMouseEnter;
            //Content.MouseLeave += OnMouseLeave;
        }

        private void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            if (CurrentworkFlow.workflowName.Equals(""))
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
            else
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
            bitmapImg.EndInit();
            ImageWorkflow.Source = bitmapImg;

            Content.Background = new ImageBrush(bitmapImg);
        }

        private void OnMouseLeave(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            if (CurrentworkFlow.workflowName.Equals(""))
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_Add.png", UriKind.Absolute);
            else
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);
            bitmapImg.EndInit();
            ImageWorkflow.Source = bitmapImg;

            Content.Background = new ImageBrush(bitmapImg);
        }


    }
}
