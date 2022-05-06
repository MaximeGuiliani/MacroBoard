using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace MacroBoard
{
    internal class BlockRunApp : Block
    {
        public string _url;
        public BlockRunApp(string Url)
        {
            base.Name = "Run App";
            _url = Url;
        }

        public override void Execute()
        {
            Process.Start(_url);
        }


    }
}
