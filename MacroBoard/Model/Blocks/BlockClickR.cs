using System;
using System.Runtime.InteropServices;
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockClickR : Block
    {


        public BlockClickR()
        {
            base.Name = "Right Click";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockClickR.png";
            base.info = "Simulate a Right mouse click in the current position of the mouse pointer.";
            base.category = Categories.Controls;

        }


        public override void Execute()
        {
            mouse_event((int)MouseEventFlags.RightDown, 0, 0, 0, 0);
            mouse_event((int)MouseEventFlags.RightUp, 0, 0, 0, 0);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}

