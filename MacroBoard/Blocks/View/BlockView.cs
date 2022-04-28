using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace MacroBoard
{
    internal class BlockView
    {
        public Button Btn_Delete { get; } = new();
        public Button Btn_Edit { get; } = new();
        public Button Btn_Up { get; } = new();
        public Button Btn_Down { get; } = new();
        private Label Lbl_Name = new();
        public Grid Content { get; } = new();
        public Block Block { get; set; }

        public BlockView(string Name, Block Block)
        {
            this.Block = Block;
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

            //Up Button
            txtBlock = new();
            txtBlock.Text = "🠕";
            thickness = new Thickness();
            thickness.Top = -3d;
            txtBlock.Margin = thickness;
            Btn_Up.Content = txtBlock;

            Btn_Up.Width = 15d;
            Btn_Up.Height = 15d;
            Btn_Up.HorizontalAlignment = HorizontalAlignment.Left;
            Btn_Up.VerticalAlignment = VerticalAlignment.Top;

            //Down Button
            txtBlock = new();
            txtBlock.Text = "🠗";
            thickness = new Thickness();
            thickness.Top = -3d;
            txtBlock.Margin = thickness;
            Btn_Down.Content = txtBlock;

            Btn_Down.Width = 15d;
            Btn_Down.Height = 15d;
            Btn_Down.HorizontalAlignment = HorizontalAlignment.Left;
            Btn_Down.VerticalAlignment = VerticalAlignment.Bottom;

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

            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Delete);
            Content.Children.Add(Btn_Edit);
            Content.Children.Add(Btn_Up);
            Content.Children.Add(Btn_Down);
        }

    }
}
