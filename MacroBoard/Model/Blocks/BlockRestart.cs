using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    internal class BlockRestart : Block
    {
        public BlockRestart()
        {
            base.Name = "Restart"
        }

        public override void Execute()
        {
            
            Process.Start("powershell.exe", "shutdown -r");

        }

       
    }
}
