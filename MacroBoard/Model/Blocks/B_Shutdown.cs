using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    internal class B_Shutdown : Block
    {
        public B_Shutdown()
        {
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", "shutdown -s");




        }
    }
}

