using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockLaunchBrowserFirefox : Block
    {
        public String address;
        public WindowStyle windowStyle;
        public int delay;

        public BlockLaunchBrowserFirefox(String address, WindowStyle windowStyle = WindowStyle.Normal, int delay = 3_000)
        {
            this.windowStyle = windowStyle;
            this.address = address;
            base.Name = "Launch Mozilla Firefox";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockLaunchFirefox.png";
            base.info = "Start the browser Firefox on a new tab with the specified address.";
            this.delay = delay;
            base.category = Categories.Browsers;

        }

        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Config.PathFromList("fireFoxPaths"));
            startInfo.Arguments = $"-new-tab {this.address}";
            Process.Start(startInfo);
            Thread.Sleep(delay);
            Process? p = getRecentProcess("firefox");
            if (p != null)
                ShowWindow(p.MainWindowHandle, (int)windowStyle);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
