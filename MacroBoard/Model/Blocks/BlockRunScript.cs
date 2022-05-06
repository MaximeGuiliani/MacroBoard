using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace MacroBoard
{
    internal class BlockRunScript : Block
    {
        public string _script;

        public BlockRunScript(string Name, string LogoUrl, string info, string script)
        {
            base.Name = "Run Script";
            this._script = script;
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", _script);
        }


    
    }
}
