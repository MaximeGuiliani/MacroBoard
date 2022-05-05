using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    internal class BlockShutdown : Block
    {
        public BlockShutdown()
        {
        base.Name = "Shut Down";
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", "shutdown -s");

        }


    }
}

