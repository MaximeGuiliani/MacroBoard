using System.Diagnostics;
using System.Threading;
using System.IO;
using static MacroBoard.Utils;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;
using System.Windows.Shapes;

namespace MacroBoard
{
    [Serializable]
    public class BlockLaunchApp : Block
    {
        public string appPath;
        public string arguments;
        /*public WindowStyle windowStyle;
        public int delay;*/


        public BlockLaunchApp(string appPath, string arguments=""/*,WindowStyle windowStyle = WindowStyle.Normal, int delay = 3_000*/)
        {
            this.appPath = appPath;
            this.arguments = arguments;
            /*this.windowStyle = windowStyle;
            this.delay = delay;*/
            base.info = "Launch the given application with optional arguments.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockRunApplication.png";
            base.Name = "Launch App";
            base.category = Categories.Applications;

        }


        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(appPath);
            startInfo.Arguments   = arguments;
            Process proc = Process.Start(startInfo);
            string name = proc.ProcessName;
            
            /*
            Thread.Sleep(delay);
            Process? p = getRecentProcess(name);
            if (p != null)
            {
                ShowWindow(p.MainWindowHandle, (int)windowStyle);
                
                System.Drawing.Rectangle rect = GetWindowRect(p.MainWindowHandle);
                MessageBox.Show(rect.Width.ToString()+" "+ rect.Height.ToString());
                System.Drawing.Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                MessageBox.Show("screen: "+screen.Width.ToString() + " " + screen.Height.ToString());

                //NativeWindow w = FromHandle(p.MainWindowHandle);

                //MessageBox.Show(name);
                //MessageBox.Show(p.ProcessName);
                //MessageBox.Show(p.MainWindowHandle.ToString());
            }
            */

        }







        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }





    }
}
