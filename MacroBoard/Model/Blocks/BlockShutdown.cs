using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class BlockShutdown : Block
    {
        public BlockShutdown()
        {
            base.info = "Shutdown the computer.";
            base.Name = "Shut Down";
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", "shutdown -s");

        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}

