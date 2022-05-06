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
        public string script;

        public BlockRunScript(string script)
        {
            base.Name = "Run Script";
            this.script = script;
        }

        public override void Execute()
        {
            Process.Start("powershell.exe", script);
        }


    
    }
}
