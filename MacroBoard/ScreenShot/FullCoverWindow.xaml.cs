using System;
using System.Windows;
using System.Windows.Input;


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
            p2 = e.GetPosition(this);
            double width  = Math.Abs(p1.X-p2.X);
            double height = Math.Abs(p1.Y - p2.Y);
            updateHiding(width, height);
            updateMainRect(width, height);
        }


        private void updateMainRect(double width, double height)
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


        private void updateHiding(double width, double height)
        {
            hide_width.Width = new GridLength(width);
            hide_height.Height = new GridLength(height);
            hide_right.Width = new GridLength(Math.Min(p1.X, p2.X));
            hide_top.Height = new GridLength(Math.Min(p1.Y, p2.Y));
        }





    }
}
