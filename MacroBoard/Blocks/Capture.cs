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


namespace MacroBoard
{
    class Capture : Block
    {

        string fileName, format, path;
        ImageFormat imageFormat = ImageFormat.Jpeg;
        int screenNumber;


        public Capture(string fileName, string format, string path, int screenNumber)
        {
            this.path = path;
            this.fileName = fileName;
            this.format = format;
            this.screenNumber = screenNumber;
            switch (format)
            {
                case ("jpeg"):
                case ("jpg"):
                    this.imageFormat = ImageFormat.Jpeg;
                    break;
                case ("tiff"):
                    this.imageFormat = ImageFormat.Tiff;
                    break;
                case ("png"):
                    this.imageFormat = ImageFormat.Png;
                    break;
                default:
                    break;
            }
        }


        public override void Execute()
        {
            int width  = Screen.AllScreens[this.screenNumber].Bounds.Width;
            int height = Screen.AllScreens[this.screenNumber].Bounds.Height;
            Bitmap captureBitmap = new Bitmap(width, height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
            captureBitmap.Save(this.path + (this.path.EndsWith(@"\") ? "" : @"\") + this.fileName + "." + this.format);
            
            //MessageBox.Show( this.path + (this.path.EndsWith(@"\")? "":@"\") + this.fileName + "." + this.format);
            //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            //captureBitmap.Save(@"C:\Users\leopaul\Desktop", ImageFormat.Jpeg);
        }

















    }
}
