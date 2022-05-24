using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard
{

    internal abstract class BlockViewModel
    {
        public Label Lbl_Name = new();
        public Grid Content { get; } = new();
        public Block Block { get; set; }

        public BlockViewModel(Block Block)
        {
            this.Block = Block;
            Setup_Btns();
            Setup_Name(Block.Name);
            Setup_Grid();
            Setup_Logo(Block.LogoUrl);
        }

        private void Setup_Name(string Name)
        {
            Lbl_Name.Content = Name;
            Lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
            Lbl_Name.VerticalAlignment = VerticalAlignment.Center;
        }

        public abstract void Setup_Btns();
        public abstract void Setup_Grid();

        private void Setup_Logo(string LogoUrl)
        {
            Image img = new();
            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri(LogoUrl, UriKind.Relative);
            bitmapImg.EndInit();
            
            img.Source = bitmapImg;

            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(img, EdgeMode.Aliased);

            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;

            Thickness thickness = new Thickness();
            thickness.Top = 12d;
            thickness.Left = 25d;

            img.Margin = thickness;
            img.Width = 32d;
            img.Height = 32d;

            Content.Children.Add(img);
        }





    }
}
