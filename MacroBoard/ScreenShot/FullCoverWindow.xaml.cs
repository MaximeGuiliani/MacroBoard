using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacroBoard.ScreenShot
{
    public partial class FullCoverWindow : Window
    {

        public Point p1;
        public Point p2;

        public FullCoverWindow()
        {
            InitializeComponent();
            DataContext = this;
        }


        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            p1 = e.GetPosition(this);
            rect.Margin = new Thickness(p1.X, p1.Y, 0, 0);
            rect.Visibility = Visibility.Visible;
            this.MouseMove += onMouseMove;
        }


        private void onMouseUp(object sender, MouseButtonEventArgs e)
        {
            p2 = e.GetPosition(this);
            this.MouseMove -= onMouseMove;
            if (p1.X-p2.X == 0 || p1.Y-p2.Y == 0) this.DialogResult = false;
            else this.DialogResult = true;
            this.Close();
        }


        private void onMouseMove(object sender, MouseEventArgs e)
        {
            Point p2 = e.GetPosition(this);
            double width  = Math.Max(p1.X-p2.X, p2.X - p1.X);
            double height = Math.Max(p1.Y - p2.Y, p2.Y - p1.Y);
            
            updateMainRect(width, height, p2);
            updateHiding(width, height, p2);
        }


        private void updateMainRect(double width, double height, Point p2)
        {
            if (p2.X > p1.X)
                if (p2.Y > p1.Y)
                    rect.Margin = new Thickness(p1.X, p1.Y, 0, 0);
                else
                    rect.Margin = new Thickness(p1.X, p1.Y - height, 0, 0);
            else
                if (p2.Y > p1.Y)
                rect.Margin = new Thickness(p1.X - width, p1.Y, 0, 0);
            else
                rect.Margin = new Thickness(p1.X - width, p1.Y - height, 0, 0);

            rect.Width = width;
            rect.Height = height;
        }


        private void updateHiding(double width, double height, Point p2)
        {
            hide_width.Width   = new GridLength(width);
            hide_height.Height = new GridLength(height);
            hide_right.Width   = new GridLength(Math.Min(p1.X, p2.X));
            hide_top.Height    = new GridLength(Math.Min(p1.Y, p2.Y));
        }





    }
}
