using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    class BlockRestart : Block
    {
        public BlockRestart()
        {
            base.info = "Restart the computer.";
            base.Name = "Restart";
        }

        public override void Execute()
        {
            
            Process.Start("powershell.exe", "shutdown -r");

        }

       
    }
}
