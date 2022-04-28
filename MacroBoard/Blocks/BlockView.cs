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
    /*
     <Grid x:Name="MainGrid" Margin="5" Width="229">
        <Label x:Name="Label_Name" Content="Test" Height="26" Margin="15,7,15,7" HorizontalAlignment="Center"/>
        <Button x:Name="Button_Up" Width="15" Height="15" HorizontalAlignment="Left" VerticalAlignment="Top">
            <TextBlock Text="🠕" Margin="0,-3,0,0"/>
        </Button>
        <Button x:Name="Button_Down" Width="15" Height="15" HorizontalAlignment="Left" VerticalAlignment="Bottom">
            <TextBlock Text="🠗" Margin="0,-3,0,0" VerticalAlignment="Stretch"/>
        </Button>
        <Button x:Name="Button_Delete" Width="15" Height="11" HorizontalAlignment="Right" VerticalAlignment="Top">
            <TextBlock Text="-" Margin="0,-6,0,0" VerticalAlignment="Stretch"/>
        </Button>
        <Button x:Name="Button_Edit" Width="30" Height="15" HorizontalAlignment="Right" VerticalAlignment="Bottom">
            <TextBlock Text="edit" Margin="0,-3,0,0" VerticalAlignment="Stretch"/>
        </Button>
      </Grid>
    */
    internal class BlockView
    {
        public Button Btn_Delete { get; } = new();
        public Button Btn_Edit { get; } = new();
        public Button Btn_Up { get; } = new();
        public Button Btn_Down { get; } = new ();
        private Label lbl_Name = new();
        public Grid Content { get; } = new();

        public BlockView(string Name)
        {
            Setup_Btns();
            Setup_Name(Name);

            Thickness thickness = new Thickness();
            thickness.Left = 5d;
            thickness.Right = 5d;
            thickness.Top = 5d;
            thickness.Bottom = 5d;
            Content.Margin = thickness;
            Content.Width = 230d;

            Content.Children.Add(lbl_Name);
            Content.Children.Add(Btn_Delete);
        }

        private void Setup_Name(string Name)
        {
            lbl_Name.Content = Name;
            lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void Setup_Btns()
        {
            TextBlock txtBlock = new();
            txtBlock.Text = "-";
            Thickness thickness = new Thickness();
            thickness.Top = -6d;
            txtBlock.Margin = thickness;
            Btn_Delete.Content = txtBlock;

            Btn_Delete.Width = 15;
            Btn_Delete.Height = 11;
            Btn_Delete.HorizontalAlignment = HorizontalAlignment.Right;
            Btn_Delete.VerticalAlignment = VerticalAlignment.Top;
        }

    }
}
