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
    internal class BlockLaunchEdgeBrowser : Block
    {
        public String address;

        public BlockLaunchEdgeBrowser(String address)
        {
            this.address = address;
            base.info = "Start the Edge browser on a new tab with the specifiedaddress.";
            base.Name = "LaunchEdgeBrowser";
            base.LogoUrl = "";
        }

        public override void Execute()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            //startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = $"microsoft-edge:{this.address}";
            Process.Start(startInfo);


        }



    }
}
