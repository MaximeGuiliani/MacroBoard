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
    internal class ClickG : Block
    {
        int x;
        int y;
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
                ((int)MouseEventFlags.LeftDown,
                 this.x,
                 this.y,
                 0,
                 0)
                ;

            //Thread.Sleep(100);

            mouse_event
                ((int)MouseEventFlags.LeftUp,
                 this.x,
                 this.y,
                 0,
                 0)
                ;
        }

        public ClickG(int x, int y)
        {
            this.x = x;
            this.y = y;

        }


    }

}
