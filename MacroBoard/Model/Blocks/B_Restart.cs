using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard.Blocks
{
    internal class B_Restart : Block
    {
        public B_Restart()
        {
    
        }

        public override void Execute()
        {
            
            Process.Start("powershell.exe", "shutdown -r");

        }

       
    }
}
