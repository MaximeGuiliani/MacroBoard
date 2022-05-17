using System.Diagnostics;
using System.Threading;
using static MacroBoard.Utils;



namespace MacroBoard
{
    public class BlockLaunchApp : Block
    {
        public string appPath;
        public string arguments;
        public WindowStyle windowStyle;
        public int delay;


        public BlockLaunchApp(string appPath, string arguments="", WindowStyle windowStyle = WindowStyle.Normal, int delay = 3_000)
        {
            this.appPath = appPath;
            this.arguments = arguments;
            this.windowStyle = windowStyle;
            this.delay = delay;
            base.info = "Launch the given application with optional arguments.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockRunApplication.png";
            base.Name = "Launch App";
        }


        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(appPath);
            startInfo.Arguments   = arguments;
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
