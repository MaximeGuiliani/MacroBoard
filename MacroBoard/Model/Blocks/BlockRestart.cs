using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class BlockRestart : Block
    {
        public BlockRestart()
        {
            base.info = "Restart the computer.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockRestart.png";
            base.Name = "Restart";
        }

        public override void Execute()
        {
            
            Process.Start("powershell.exe", "shutdown -r");

        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
