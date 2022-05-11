using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace MacroBoard
{
    public class BlockClickR : Block
    {

        public BlockClickR()
        {
            base.Name = "Right Click";
            base.info = "Simulate a Right mouse click in the current position of the mouse pointer.";
        }


        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }


        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public override void Execute()
        {
            mouse_event
                ((int)MouseEventFlags.RightDown,
                 0,
                 0,
                 0,
                 0)
                ;

            mouse_event
                ((int)MouseEventFlags.RightUp,
                 0,
                 0,
                 0,
                 0)
                ;
        }



    }

}

