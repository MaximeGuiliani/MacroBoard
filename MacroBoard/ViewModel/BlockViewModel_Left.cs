using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard
{
    internal class BlockViewModel_Left : BlockViewModel
    {
        public Button Btn_Add { get; } = new();

        public BlockViewModel_Left(Block Block) : base(Block) { }

        override public void Setup_Btns()
        {
            TextBlock txtBlock = new();
            //Add To Workflow Button
            txtBlock.Text = "+";
            Thickness thickness = new Thickness();
            thickness.Top = -6d;
            txtBlock.Margin = thickness;
            Btn_Add.Content = txtBlock;

            Btn_Add.Width = 15d;
            Btn_Add.Height = 11d;
            Btn_Add.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Add.VerticalAlignment = VerticalAlignment.Top;
        }

        public override void Setup_Grid()
        {
            Thickness thickness = new Thickness();
            thickness.Left = 8d;
            thickness.Right = 8d;
            thickness.Bottom = 8d;
            Content.Margin = thickness;
            Content.Width = 330d;
            Content.Height = 60d;

            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri("../../../Resources/block.png", UriKind.Relative);
            bitmapImg.EndInit();

            Content.Background = new ImageBrush(bitmapImg);

            Content.Children.Add(Lbl_Name);
            Content.Children.Add(Btn_Add);
        }
    }
}
