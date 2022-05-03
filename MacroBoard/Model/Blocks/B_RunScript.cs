using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace MacroBoard.Blocks
{
    internal class B_RunScript : Block
    {
        private string _script;
        public B_RunScript(string Name, string LogoUrl, string info, string script)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;
            this._script = script;
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", _script);
        }
    }
}
