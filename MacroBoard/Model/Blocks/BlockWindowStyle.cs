using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockWindowStyle : Block
    {
        public WindowStyle windowStyle;

        public BlockWindowStyle(WindowStyle windowStyle = WindowStyle.Normal)
        {
            base.Name = "Window Style";
            base.info = "Set the style of the foreground window";
            this.windowStyle = windowStyle;
        }


        public override void Execute()
        {
            ShowWindow(GetForegroundWindow(), (int)windowStyle);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
