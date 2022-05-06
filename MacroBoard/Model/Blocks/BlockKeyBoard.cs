﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MacroBoard
{
    internal class BlockKeyBoard : Block
    {
        public string _shortCut;
        public BlockKeyBoard(string ShortCut)
        {
            base.Name = "Keyboard";
            this._shortCut = ShortCut;
        }


        public override void Execute()
        {
            SendKeys.SendWait(_shortCut);

        }
    }
}
