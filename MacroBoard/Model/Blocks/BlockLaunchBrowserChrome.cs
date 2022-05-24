using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;
using MacroBoard;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockLaunchBrowserChrome : Block
    {

        public String address;
        public WindowStyle windowStyle;
        public int delay;


        public BlockLaunchBrowserChrome(String address, WindowStyle windowStyle=WindowStyle.Normal, int delay = 3_000)
        {
            this.address = address;
            this.windowStyle = windowStyle;
            this.delay = delay;
            base.Name = "Launch Google Chrome";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockLaunchChrome.png";
            base.info = "Start the browser Chrome on a new tab with the specified address.";
            base.category = Categories.Browsers;
        }


        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Config.PathFromList("chromePaths"));
            startInfo.Arguments = $"-d {this.address}";
            Process.Start(startInfo);
            Thread.Sleep(delay);
            Process? p = getRecentProcess("chrome");
            if (p != null)
                ShowWindow(p.MainWindowHandle, (int)windowStyle);
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
