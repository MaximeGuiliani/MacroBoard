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


        public BlockLaunchApp(string appPath, string arguments="")
        {
            base.info = "Launch the given application with optional arguments.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockRunApplication.png";
            base.Name = "Launch App";
            base.category = Categories.Applications;
            this.appPath = appPath;
            this.arguments = arguments;
        }


        public override void Execute()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(appPath);
            startInfo.Arguments   = arguments;
            Process proc = Process.Start(startInfo);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }





    }
}
