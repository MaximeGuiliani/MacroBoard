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
    internal class BlockLaunchBrowserChromex86 : Block
    {
        public String address;

        public BlockLaunchBrowserChromex86(String address)
        {
            this.address = address;
            base.Name = "LaunchBrowserChrome";
            base.LogoUrl = "";
            base.info = "Start the browser Chrome (.exe in folder x86) on a new tab with the specified address.";
        }

        public override void Execute()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Normal

                ;
            startInfo.Arguments = $"-d {this.address}";
            Process.Start(startInfo);


        }






    }
}
