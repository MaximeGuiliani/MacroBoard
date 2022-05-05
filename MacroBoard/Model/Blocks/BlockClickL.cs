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
    internal class BlockClickL : Block
    {
        //int x;
        //int y;
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public BlockClickL()
        {
            base.Name = "Left Click"
            //this.x = x;
            //this.y = y;
        }


        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //static extern bool SetCursorPos(int x, int y);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);



        public override void Execute()
        {
            //SetCursorPos(this.x, this.y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        }

    }

}
