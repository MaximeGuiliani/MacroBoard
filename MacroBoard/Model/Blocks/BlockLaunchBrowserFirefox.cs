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


namespace MacroBoard
{
    internal class BlockLaunchBrowserFirefox : Block
    {
        public String address;

        public BlockLaunchBrowserFirefox(String address)
        {
            this.address = address;
            base.Name = "LaunchBrowser Firefox";
            base.LogoUrl = "";
            base.info = "Lancer le navigateur Firefox sur une url";
        }

        public override void Execute()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Mozilla Firefox\firefox.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = $"-new-tab {this.address}";
            Process.Start(startInfo);

        }


    }
}
