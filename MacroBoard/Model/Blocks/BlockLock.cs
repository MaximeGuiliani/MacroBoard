using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static MacroBoard.Utils;

namespace MacroBoard
{
    public class BlockLock : Block
    {
        
        public BlockLock()
        {
            base.info = "Lock the screen of the computer.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockLock.png";
            base.Name = "Lock";
            base.category = Categories.System;

        }


        public override void Execute()
        {
            LockWorkStation();
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
