using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard
{
    internal class WorkflowView
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


        private void Setup_Name(string Name)
        {
            Lbl_Name.Content = Name;
            Lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
            Lbl_Name.VerticalAlignment = VerticalAlignment.Center;
        }


        private void Setup_Btns()
        {

            //Main Button
            Btn_Main.Background = Brushes.Transparent;
            Btn_Main.BorderThickness = new Thickness(0, 0, 0, 0);

            Btn_Main.Width = 100d;
            Btn_Main.Height = 100d;


            //Delete Button
            TextBlock txtBlock = new();

            txtBlock.Text = "🚫";
            txtBlock.Foreground = Brushes.Red;
            txtBlock.FontSize = 25d;


            Btn_Delete.Content = txtBlock;
            Btn_Delete.Background = Brushes.Transparent;
            Btn_Delete.BorderThickness = new Thickness(0, 0, 0, 0);
            Btn_Delete.Width = 30d;
            Btn_Delete.Height = 30d;
            Btn_Delete.HorizontalAlignment = HorizontalAlignment.Left;
            Btn_Delete.VerticalAlignment = VerticalAlignment.Top;


            //Edit Button
            txtBlock = new();
            txtBlock.Text = "★";
            txtBlock.Foreground = Brushes.Yellow;
            txtBlock.FontSize = 25d;

            Btn_Fav.Content = txtBlock;
            Btn_Fav.Background = Brushes.Transparent;
            Btn_Fav.BorderThickness = new Thickness(0, 0, 0, 0);

            Btn_Fav.Width = 30d;
            Btn_Fav.Height = 30d;
            Btn_Fav.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Fav.VerticalAlignment = VerticalAlignment.Top;
        }

        private void Setup_Grid()
        {

            Content.Width = 100d;
            Content.Height = 100d;

            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();

            bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V3.png", UriKind.Absolute);

            bitmapImg.EndInit();
            ImageWorkflow.Source = bitmapImg;

            Content.Background = new ImageBrush(bitmapImg);

            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Main);
            Content.Children.Add(Btn_Delete);
            Content.Children.Add(Btn_Fav);


            Content.MouseEnter += OnMouseEnter;
            Content.MouseLeave += OnMouseLeave;
        }

        private void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            if (CurrentworkFlow.workflowName.Equals(""))
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_Add_MouseOver.png", UriKind.Absolute);
            else
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_MouseOver.png", UriKind.Absolute);
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
                bitmapImg.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Button_WorkFlow_V2.png", UriKind.Absolute);
            bitmapImg.EndInit();
            ImageWorkflow.Source = bitmapImg;

            Content.Background = new ImageBrush(bitmapImg);
        }


    }
}
