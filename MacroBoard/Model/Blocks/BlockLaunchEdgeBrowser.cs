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
    internal class LaunchEdgeBrowser : Block
    {
        String address;

        public LaunchEdgeBrowser(String address)
        {
            this.address = address;
            base.Name = "LaunchEdgeBrowser";
            base.LogoUrl = "";
            base.info = "Lancer le navigateur Edge sur une url";
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
