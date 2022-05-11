using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MacroBoard
{
    internal class BlockLock : Block
    {
        
        public BlockLock()
        {
            base.info = "Lock the screen of the computer.";
            base.Name = "Lock";
        }
        public override void Execute()
        {
            [DllImport("user32")]
            static extern void LockWorkStation();
            LockWorkStation();
        }
        
    }
}
