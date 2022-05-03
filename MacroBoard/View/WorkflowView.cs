using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard.View
{
    internal class WorkflowView
    {
        public Button Btn_Delete { get; } = new();
        public Image ImageWorkflow { get; } = new();

        public Button Btn_Edit { get; } = new();

        private Label Lbl_Name = new();
        public Grid Content { get; } = new();
        public WorkFlow workFlow { get; set; }

        public WorkflowView(string Name, WorkFlow workFlow)
        {

            this.workFlow = workFlow;
            Setup_Btns();
            Setup_Name(Name);
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
            TextBlock txtBlock = new();
            //Delete Button
            txtBlock.Text = "-";
            Thickness thickness = new Thickness();
            thickness.Top = -6d;
            txtBlock.Margin = thickness;
            Btn_Delete.Content = txtBlock;

            Btn_Delete.Width = 15d;
            Btn_Delete.Height = 11d;
            Btn_Delete.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Delete.VerticalAlignment = VerticalAlignment.Top;




            //Edit Button
            txtBlock = new();
            txtBlock.Text = "edit";
            thickness = new Thickness();
            thickness.Top = -3d;
            txtBlock.Margin = thickness;
            Btn_Edit.Content = txtBlock;

            Btn_Edit.Width = 30d;
            Btn_Edit.Height = 15d;
            Btn_Edit.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Edit.VerticalAlignment = VerticalAlignment.Bottom;
        }

        private void Setup_Grid()
        {
            Thickness thickness = new Thickness();
            thickness.Left = 8d;
            thickness.Right = 8d;
            thickness.Bottom = 8d;
            Content.Margin = thickness;
            Content.Width = 230d;
            Content.Height = 35d;

            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri("../../../images/macro.png", UriKind.Relative);
            bitmapImg.EndInit();

            Content.Background = new ImageBrush(bitmapImg);

            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Delete);
            Content.Children.Add(Btn_Edit);
        }


    }
}
