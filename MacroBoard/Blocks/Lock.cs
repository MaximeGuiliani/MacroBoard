using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MacroBoard
{
    internal class Lock : Block
    {
        public override void Execute()
        {
            [DllImport("user32")]
            static extern void LockWorkStation();
            LockWorkStation();
        }
    }
}
