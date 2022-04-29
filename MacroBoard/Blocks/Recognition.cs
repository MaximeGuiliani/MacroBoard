using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Image = System.Drawing.Image;
using System.IO;

namespace MacroBoard
{
    internal class Recognition : Block
    {

        int screenNumber;
        string imagePath;
        int offSetX;
        int offSetY;

        public Recognition(string ImagePath, int offSetX=0, int offSetY=0, int screenNumber=0)
        {
            this.imagePath = ImagePath;
            this.offSetX = offSetX;
            this.offSetY = offSetY;
            this.screenNumber = screenNumber;
        }

        private Rectangle searchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride; //décalage +1 ligne (1 Stride = 3*largeur car rgb +offset )
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            System.Drawing.Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0; //pointeur sur premiere couleur de 1er pixel
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++) //x,y,i,j utile pour éviter les offset et pour le return 
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers. (comme au début de la ligne)
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }
                    //ici fin d'une ligne big 
                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            return location;
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);


        private Bitmap capture(){
            int width = Screen.AllScreens[this.screenNumber].Bounds.Width;
            int height = Screen.AllScreens[this.screenNumber].Bounds.Height;
            Bitmap captureBitmap = new Bitmap(width, height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
            return captureBitmap;
        }


        public override void Execute()
        {
            Bitmap bitmap1 = new Bitmap(this.imagePath);
            Bitmap bitmap2 = capture();
            double tolerance;
            bool found = false;
            for (tolerance = 0; tolerance < 1; tolerance += 0.04)
            {
                Rectangle location = searchBitmap(bitmap1, bitmap2, tolerance);
                if (location.Width != 0)
                {
                    found = true;
                    SetCursorPos(location.X+(location.Width/2)+this.offSetX, location.Y+(location.Height/2)+this.offSetY);
                    //MessageBox.Show($"Tolerance:{tolerance}    w:{location.Width}    h:{location.Height}    X: {location.X}   Y: {location.Y}");
                    break;
                }
            }
            if (!found)
            {
                //MessageBox.Show("pas trouvé :(");
            }
        }



        /*******************************************************************************
         * 
         * TODO: réduire la zone de recherche si possible avec: pt_up_left pt_down_right
         * TODO: ajouter "scaling" ou cas ou un scaling est utilisé par l'OS ?
         * 
         ******************************************************************************/






    }
}
