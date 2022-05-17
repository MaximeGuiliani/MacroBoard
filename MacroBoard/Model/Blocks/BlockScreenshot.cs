using System.Windows.Forms;
using System.Drawing;
using static MacroBoard.Utils;



namespace MacroBoard{
    public class BlockScreenshot : Block
    {

        public string folderPath;
        public string fileName;
        public int screenNumber;
        private int actualScreenNumber;


        public BlockScreenshot(string folderPath, string fileName, int screenNumber)
        {
            base.info = "Take a screenshot.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockScreenshot.png";
            base.Name = "Screenshot";
            this.folderPath = folderPath;
            this.fileName = fileName;
            this.screenNumber = screenNumber;
            this.actualScreenNumber = (screenNumber >= Screen.AllScreens.Length) ? 0 : screenNumber ;
        }


        public override void Execute()
        {
            Rectangle screen = Screen.AllScreens[actualScreenNumber].Bounds;
            Bitmap captureBitmap = new Bitmap(screen.Width, screen.Height);
            Graphics screenShotGraphics = Graphics.FromImage(captureBitmap);
            screenShotGraphics.CopyFromScreen(screen.X, screen.Y, 0, 0, new Size(screen.Width, screen.Height));
            captureBitmap.Save(concatPathWithFileName(folderPath,fileName));
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }














    }
}
