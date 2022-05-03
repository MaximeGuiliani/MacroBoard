using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MacroBoard.Blocks
{
    internal class B_KeyBoardShortCut : Block
    {
        private string _shortCut;
        public B_KeyBoardShortCut(string Name, string LogoUrl, string info, string ShortCut)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;
            this._shortCut = ShortCut;
        }
        public override void Execute()
        {
            SendKeys.SendWait(_shortCut);

        }
    }
}
