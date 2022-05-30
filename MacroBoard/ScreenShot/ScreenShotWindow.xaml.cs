using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MacroBoard.Utils;

namespace MacroBoard.ScreenShot
{
    /// <summary>
    /// Interaction logic for ScreenShot.xaml
    /// </summary>
    public partial class ScreenShotWindow : Window
    {

        public string filepath = "";


        public ScreenShotWindow()
        {
            InitializeComponent();
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Click_ScreenShot(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0;
            FullCoverWindow w = new FullCoverWindow();
            if(w.ShowDialog() == true) takeScreenShot(w.p1, w.p2);
            this.DialogResult = true;
            this.Close();
        }


        private void takeScreenShot(System.Windows.Point p1, System.Windows.Point p2)
        {
            System.Drawing.Rectangle screen = Screen.FromHandle(gethWnd()).Bounds;
            int width = (int)Math.Max((p1.X - p2.X), (p2.X - p1.X));
            int height = (int)Math.Max((p1.Y - p2.Y), (p2.Y - p1.Y));
            Bitmap captureBitmap = new Bitmap(width, height);
            Graphics screenShotGraphics = Graphics.FromImage(captureBitmap);
            screenShotGraphics.CopyFromScreen(screen.X+(int)(Math.Min(p1.X, p2.X)), screen.Y+(int)(Math.Min(p1.Y, p2.Y)), 0, 0, new System.Drawing.Size(width, height));
            save(captureBitmap);
        }


        private IntPtr gethWnd()
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
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                captureBitmap.Save(saveFileDialog.FileName);
                this.filepath = saveFileDialog.FileName;
            }
        }






    }
}
