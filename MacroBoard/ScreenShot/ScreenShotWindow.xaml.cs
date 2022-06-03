using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using static MacroBoard.Utils;

namespace MacroBoard.ScreenShot
{
    public partial class ScreenShotWindow : Window
    {

        public string filepath;
        Rectangle screen;


        public ScreenShotWindow()
        {
            filepath = "";
            screen = Screen.FromHandle(getThisHandle()).Bounds;
            InitializeComponent();
        }


        /* Pour déplacer la fenetre avec un windows style = None */
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }


        private void Click_ScreenShot(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0;
            this.screen = Screen.FromHandle(getThisHandle()).Bounds;
            FullCoverWindow w = new FullCoverWindow();
            w.Top    = screen.Top;
            w.Left   = screen.Left;
            w.Width  = screen.Width;
            w.Height = screen.Height;
            if (w.ShowDialog() == true)
            {
                double ratio = GetDpiForWindow(getThisHandle())/96.0d;
                takeScreenShot( new System.Windows.Point(w.p1.X*ratio, w.p1.Y*ratio) , new System.Windows.Point(w.p2.X*ratio, w.p2.Y*ratio));
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
            this.Close();
        }


        private void takeScreenShot(System.Windows.Point p1, System.Windows.Point p2)
        {
            int width  = (int)Math.Abs(p1.X - p2.X);
            int height = (int)Math.Abs(p1.Y - p2.Y);
            int X = screen.X + (int)Math.Min(p1.X, p2.X);
            int Y = screen.Y + (int)Math.Min(p1.Y, p2.Y);
            Bitmap captureBitmap = new Bitmap(width, height);
            Graphics screenShotGraphics = Graphics.FromImage(captureBitmap);
            screenShotGraphics.CopyFromScreen(X, Y, 0, 0, new System.Drawing.Size(width, height));
            save(captureBitmap);
        }


        private IntPtr getThisHandle()
        {
            var wih = new System.Windows.Interop.WindowInteropHelper(this);
            return wih.Handle;
        }


        private void save(Bitmap captureBitmap)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "filename";
            saveFileDialog.Filter = "jpg (*.jpg)|*.jpg" + "|image (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg" +"|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                captureBitmap.Save(saveFileDialog.FileName);
                this.filepath = saveFileDialog.FileName;
            }
        }






    }
}
