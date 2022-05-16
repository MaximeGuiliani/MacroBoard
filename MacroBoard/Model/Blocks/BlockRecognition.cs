using System.Collections.Generic;
using System.Drawing.Imaging;
using OpenCvSharp.Extensions;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp;
using System.Linq;
using static MacroBoard.Utils;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text;


namespace MacroBoard
{
    public class BlockRecognition : Block
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
        public Rectangle screen;
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
            this.screenNumber   = screenNumber; //NOTE: ne pas utilsier dans le code
            this.offSetX        = offSetX;
            this.offSetY        = offSetY;
            this.scale          = scale;
            this.loop           = loop;
            this.debugMode      = debugMode;
            this.matchModes     = matchModes;
            //computed from constructor
            this.screen         = Screen.AllScreens[(screenNumber>= Screen.AllScreens.Length)? 0:screenNumber].Bounds;
            this.rectOfInterest = new Rect(xInterest, yInterest, widthInterest <= 0 ? this.screen.Width - xInterest : widthInterest, heightInterest <= 0 ? this.screen.Height - yInterest : heightInterest);
            int[] pourcentages  = { 100, 125, 150, 175, 200 };
            var scales          = (from p1 in pourcentages from p2 in pourcentages where p1 != p2 select (double)p1 / (double)p2).Append(1);
            this.tryScales      = (loop) ? scales : new double[] { scale };
            this.debugDirPath = Config.PathFromString("DebugPath");
        }


        public override void Execute()
        {
            Mat image    = (debugMode) ? new Mat(FileScreenShot()) : BitmapConverter.ToMat(BitmapScreenShot());
            Mat template = new Mat(templatePath);

            (double maxVal, OpenCvSharp.Point? maxLoc, double scale) found = find(image, template);

            OpenCvSharp.Point rectPt1 = new OpenCvSharp.Point((found.maxLoc?.X * ((0 < found.scale && found.scale < 1) ? (1 / found.scale) : 1) + rectOfInterest.X).GetValueOrDefault(),
                                                              (found.maxLoc?.Y * ((0 < found.scale && found.scale < 1) ? (1 / found.scale) : 1) + rectOfInterest.Y).GetValueOrDefault());
            OpenCvSharp.Point rectPt2 = new OpenCvSharp.Point(rectPt1.X + ((1 < found.scale) ? template.Width / found.scale : template.Width),
                                                              rectPt1.Y + ((1 < found.scale) ? template.Height / found.scale : template.Height));

            OpenCvSharp.Point CirclePt = new OpenCvSharp.Point(rectPt1.X + ((1 < found.scale) ? template.Width / found.scale : template.Width) / 2 + offSetX,
                                                               rectPt1.Y + ((1 < found.scale) ? template.Height / found.scale : template.Height) / 2 + offSetY);
            SetCursorPos(CirclePt.X, CirclePt.Y);

            if (debugMode) Debug(image, rectPt1, rectPt2, CirclePt, found);

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
            Bitmap screenShotBitmap = new Bitmap(screen.Width, screen.Height, PixelFormat.Format32bppRgb);
            Graphics screenShotGraphics = Graphics.FromImage(screenShotBitmap);
            // NOTE: CopyFromScreen() possede un overload avec gestion des couleurs si besoin
            screenShotGraphics.CopyFromScreen(screen.X, screen.Y, 0, 0, new System.Drawing.Size(screen.Width, screen.Height));
            return screenShotBitmap;
        }


        private string FileScreenShot()
        {
            string filePath = this.debugDirPath + @$"macroboard_{GetType().Name}_screenshot.png";
            BitmapScreenShot().Save(filePath);
            return filePath;
        }


        private void Debug(Mat image, OpenCvSharp.Point rectPt1, OpenCvSharp.Point rectPt2, OpenCvSharp.Point CirclePt, (double, OpenCvSharp.Point?, double scale) found)
        {
            Mat overlay = image.Clone();
            double alpha = 0.4d;
            Cv2.Rectangle(overlay, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(850, 112), Scalar.White, -1);
            Cv2.Rectangle(overlay, rectPt1, rectPt2, Scalar.Red, -1);
            Cv2.AddWeighted(overlay, alpha, image, 1 - alpha, 0, image);
            //
            Cv2.Rectangle(image, rectPt1, rectPt2, Scalar.Red, 1);
            Cv2.Circle(image, CirclePt, 1, Scalar.Purple, 5);
            //
            Cv2.PutText(image, $"screenNumber: {screenNumber}; x_interest: {xInterest}; y_interest: {yInterest}; w_Interest: {widthInterest}; h_Interest: {heightInterest}; scale: {scale}", new OpenCvSharp.Point(2, 20), HersheyFonts.HersheySimplex, 0.6, Scalar.DarkBlue);
            Cv2.PutText(image, $"Loop: {loop}; DebugMode: {debugMode}; Method: {matchModes}", new OpenCvSharp.Point(2, 40), HersheyFonts.HersheySimplex, 0.6, Scalar.DarkBlue);
            Cv2.PutText(image, $"w_screen: {screen.Width}; h_screen: {screen.Height}", new OpenCvSharp.Point(2, 70), HersheyFonts.HersheySimplex, 0.6, Scalar.DarkBlue);
            Cv2.PutText(image, $"rect_interest_x: {rectOfInterest.X}; rect_interest_y: {rectOfInterest.Y}; rect_interest_w: {rectOfInterest.Width}; rect_interest_h: {rectOfInterest.Height}", new OpenCvSharp.Point(2, 90), HersheyFonts.HersheySimplex, 0.6, Scalar.DarkBlue);
            Cv2.PutText(image, $"nb_tried_scales: {tryScales.Count()} found_scale: {found.scale}", new OpenCvSharp.Point(2, 110), HersheyFonts.HersheySimplex, 0.6, Scalar.DarkBlue);
            //
            image.SaveImage(this.debugDirPath + @$"macroboard_{GetType().Name}_screenshot_annotated.png");
            //Cv2.ImShow("resizedImageCrop", resizedImageCrop);
            //Cv2.ImShow("resizedTemplate", resizedTemplate);
            //Cv2.ImShow("image", image);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }






    }
}
//TODO: check rect of interest ne dépasse pas de l'image