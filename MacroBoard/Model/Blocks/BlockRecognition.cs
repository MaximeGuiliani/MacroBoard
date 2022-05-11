using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing.Imaging;
using System.Threading;


namespace MacroBoard
{
    class BlockRecognition : Block
    {
        //direct from constructor
        public string templatePath;
        public int xInterest, yInterest, heightInterest, widthInterest;
        public int screenNumber;
        public int offSetX, offSetY;
        public double scale;
        public bool loop;
        public bool debugMode;
        public TemplateMatchModes matchModes;
        //computed from constructor
        public int widthScreen;
        public int heightScreen;
        public Rect rectOfInterest;
        public IEnumerable<double> tryScales;
        public string debugDirPath;


        public BlockRecognition(string templatePath,
                      int xInterest = 0, int yInterest = 0, int heightInterest = 0, int widthInterest = 0,
                      int screenNumber = 0,
                      int offSetX = 0, int offSetY = 0,
                      double scale = 1, bool loop = false,
                      bool debugMode = false,
                      TemplateMatchModes matchModes = TemplateMatchModes.CCoeffNormed)
        {
            //heritage
            base.Name = "Recognize image";
            base.info = "Recognize a given control screenshot in the current screen and set the cursor position in its center.";
            //direct from constructor
            this.templatePath   = templatePath;
            this.xInterest      = xInterest;
            this.yInterest      = yInterest;
            this.heightInterest = heightInterest;
            this.widthInterest  = widthInterest;
            this.screenNumber   = screenNumber;
            this.offSetX        = offSetX;
            this.offSetY        = offSetY;
            this.scale          = scale;
            this.loop           = loop;
            this.debugMode      = debugMode;
            this.matchModes     = matchModes;
            //computed from constructor
            this.heightScreen   = Screen.AllScreens[screenNumber].Bounds.Height;
            this.widthScreen    = Screen.AllScreens[screenNumber].Bounds.Width;
            this.rectOfInterest = new Rect(xInterest, yInterest, widthInterest <= 0 ? this.widthScreen - xInterest : widthInterest, heightInterest <= 0 ? this.heightScreen - yInterest : heightInterest);
            int[] pourcentages  = { 100, 125, 150, 175, 200 };
            var scales          = (from p1 in pourcentages from p2 in pourcentages where p1 != p2 select (double)p1 / (double)p2).Append(1);
            this.tryScales      = (loop) ? scales : new double[] { scale };
            this.debugDirPath   = $@"C:\Users\{Environment.GetEnvironmentVariable("USERNAME")}\Documents\";
        }


        //NOTE: les valeurs affichées correspondent au nombre de pixels sans l'échelonnage appliqué (ex:150%)
        public override void Execute()
        {
            Mat image = (debugMode) ? new Mat(FileScreenShot()) : BitmapConverter.ToMat(BitmapScreenShot());
            Mat template = new Mat(templatePath);

            (double maxVal, OpenCvSharp.Point? maxLoc, double scale) found = find(image, template);


            OpenCvSharp.Point rectPt1 = new OpenCvSharp.Point((found.maxLoc?.X * ((0 < found.scale && found.scale < 1) ? (1 / found.scale) : 1) + rectOfInterest.X).GetValueOrDefault(),
                                                               (found.maxLoc?.Y * ((0 < found.scale && found.scale < 1) ? (1 / found.scale) : 1) + rectOfInterest.Y).GetValueOrDefault());
            OpenCvSharp.Point rectPt2 = new OpenCvSharp.Point(rectPt1.X + ((1 < found.scale) ? template.Width / found.scale : template.Width),
                                                              rectPt1.Y + ((1 < found.scale) ? template.Height / found.scale : template.Height));
            Cv2.Rectangle(image, rectPt1, rectPt2, Scalar.Red, 1);


            OpenCvSharp.Point CirclePt = new OpenCvSharp.Point(rectPt1.X + ((1 < found.scale) ? template.Width / found.scale : template.Width) / 2 + offSetX,
                                                                rectPt1.Y + ((1 < found.scale) ? template.Height / found.scale : template.Height) / 2 + offSetY);
            Cv2.Circle(image, CirclePt, 1, Scalar.Purple, 5);
            SetCursorPos(CirclePt.X, CirclePt.Y);


            if (debugMode) image.SaveImage(this.debugDirPath + @$"macroboard_{GetType().Name}_screenshot_annotated.png");
            //Cv2.ImShow("resizedImageCrop", resizedImageCrop);
            //Cv2.ImShow("resizedTemplate", resizedTemplate);
            //Cv2.ImShow("image", image);
        }



        private (double, OpenCvSharp.Point?, double) find(Mat image, Mat template)
        {
            Mat imageCrop = image[this.rectOfInterest];

            (double maxVal, OpenCvSharp.Point? maxLoc, double scale) found = (-1, null, -1);
            Mat resizedImageCrop = new Mat();
            Mat resizedTemplate = new Mat();
            Mat result;
            foreach (double scale in tryScales)
            {
                if (0 < scale && scale < 1)
                {
                    Cv2.Resize(imageCrop, resizedImageCrop, new OpenCvSharp.Size(imageCrop.Width * scale, imageCrop.Height * scale));
                    result = resizedImageCrop.MatchTemplate(template, this.matchModes);
                }
                else if (1 < scale)
                {
                    //MessageBox.Show($"wo:{template.Width} w:{template.Width / scale}  ho:{template.Height}  h:{template.Height / scale}  scale: {scale}");
                    Cv2.Resize(template, resizedTemplate, new OpenCvSharp.Size(template.Width / scale, template.Height / scale));
                    result = imageCrop.MatchTemplate(resizedTemplate, this.matchModes);
                }
                else
                {
                    result = imageCrop.MatchTemplate(template, this.matchModes);
                }

                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;
                result.MinMaxLoc(out minVal, out maxVal, out minLoc, out maxLoc);

                if (found.scale < 0 || found.maxVal < maxVal) found = (maxVal, maxLoc, scale);
            }
            return found;
        }


        private Bitmap BitmapScreenShot()
        {
            Bitmap screenShotBitmap = new Bitmap(widthScreen, heightScreen, PixelFormat.Format32bppRgb);
            Graphics screenShotGraphics = Graphics.FromImage(screenShotBitmap);
            screenShotGraphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(widthScreen, heightScreen));
            return screenShotBitmap;
        }


        private string FileScreenShot()
        {
            string filePath = this.debugDirPath + @$"macroboard_{GetType().Name}_screenshot.png";
            BitmapScreenShot().Save(filePath);
            return filePath;
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);



    }
}


//TODO: check rect of interest ne dépasse pas de l'image
