using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;


namespace MacroBoard{
    public class Capture : Block
    {
        string fileName;
        int screenNumber;


        public Capture(string fileName, int screenNumber)
        {
            this.fileName = fileName;
            this.screenNumber = screenNumber;
        }


        public override void Execute()
        {
            int width  = Screen.AllScreens[this.screenNumber].Bounds.Width;
            int height = Screen.AllScreens[this.screenNumber].Bounds.Height;
            Bitmap captureBitmap = new Bitmap(width, height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
            captureBitmap.Save (this.fileName);
            
            //MessageBox.Show( this.path + (this.path.EndsWith(@"\")? "":@"\") + this.fileName + "." + this.format);
            //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            //captureBitmap.Save(@"C:\Users\leopaul\Desktop", ImageFormat.Jpeg);
        }

















    }
}
