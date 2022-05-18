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
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockSetCursor : Block
    {

        public int x;
        public int y;


        public BlockSetCursor(int x, int y)
        {
            base.info = "Set the mouse cursor position at the given coordinate.";
            base.Name = "Set Cursor";
            this.x = x;
            this.y = y;
        }


        public override void Execute()
        {
            SetCursorPos(this.x, this.y);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }




    }
}
