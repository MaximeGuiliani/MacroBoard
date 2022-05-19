using System;
using System.Diagnostics;
using System.Threading;
using static MacroBoard.Utils;


namespace MacroBoard
{
    public class BlockLaunchBrowserEdge : Block
    {

        public String address;
        public WindowStyle windowStyle;
        public int delay;


        public BlockLaunchBrowserEdge(String address, WindowStyle windowStyle = WindowStyle.Normal, int delay=3_000)
        {
            base.info = "Start the Edge browser on a new tab with the specifiedaddress.";
            base.Name = "Launch Microsoft Edge";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockLaunchEdge.png";
            this.address = address;
            this.windowStyle = windowStyle;
            this.delay = delay;
            base.category = Categories.Browsers;
        }


        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Config.PathFromList("edgePaths"));
            startInfo.Arguments = this.address;
            Process.Start(startInfo);
            Thread.Sleep(delay);
            Process? p = getRecentProcess("msedge", true);
            if (p != null)
                ShowWindow(p.MainWindowHandle, (int)windowStyle);
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }




    }
}
