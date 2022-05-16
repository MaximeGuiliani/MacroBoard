using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MacroBoard
{
    public class BlockKeyBoard : Block
    {
        public string _shortCut;
        public BlockKeyBoard(string ShortCut)
        {
            base.Name = "Keyboard";
            base.info = "Simulate user input with the keyboard including ctrl, alt and shift.";
            this._shortCut = ShortCut;
        }


        public override void Execute()
        {
            SendKeys.SendWait(_shortCut);

        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
