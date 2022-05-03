using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace MacroBoard.Blocks
{
    internal class B_RunApp : Block
    {
        private string _url;
        public B_RunApp(string Name, string LogoUrl, string info, string Url)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;
            _url = Url;
        }

        public override void Execute()
        {
            Process.Start(_url);
        }
    }
}
