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
    internal class TEST : Block
    {
        public TEST(string Name, string LogoUrl, string info)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;
        }

        public override void Execute()
        {
            /*
            Process.Start(@"C:\WINDOWS\system32\notepad.exe");
            Process.Start("shutdown.exe");
            Thread.Sleep(1000);
            SendKeys.SendWait("PAPA");
            SendKeys.SendWait("^({ESC}E)");
            */
        }
    }
}
