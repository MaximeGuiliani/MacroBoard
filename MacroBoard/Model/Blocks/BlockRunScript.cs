using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace MacroBoard
{
    public class BlockRunScript : Block
    {
        public string script;

        public BlockRunScript(string script)
        {
            base.info = "Execute a script with powershell.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockRunScript.png";
            base.Name = "Run Script";
            this.script = script;
            base.category = Categories.System;

        }

        public override void Execute()
        {
            Process.Start("powershell.exe", script);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
