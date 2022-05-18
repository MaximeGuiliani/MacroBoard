using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static MacroBoard.Utils;

namespace MacroBoard
{
    [Serializable]
    public class BlockLock : Block
    {
        
        public BlockLock()
        {
            base.info = "Lock the screen of the computer.";
            base.Name = "Lock";
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
