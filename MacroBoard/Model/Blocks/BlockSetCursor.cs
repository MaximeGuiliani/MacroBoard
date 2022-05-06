using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;


namespace MacroBoard
{
    class BlockSetCursor : Block
    {
        public int x;
        public int y;


        public BlockSetCursor(int x, int y)
        {
            base.Name = "Set Cursor";
            this.x = x;
            this.y = y;
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);


        public override void Execute()
        {
            SetCursorPos(this.x, this.y);
        }





    }
}
