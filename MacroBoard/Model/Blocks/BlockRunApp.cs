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
        public string url;
        public BlockRunApp(string url)
        {
            base.Name = "Run App";
            url = url;
        }

        public override void Execute()
        {
            Process.Start(url);
        }


    }
}
