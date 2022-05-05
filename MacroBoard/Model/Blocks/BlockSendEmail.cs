using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    internal class BlockSendEmail : Block
    {
        string body;
        string to;
        string subject;
        public BlockSendEmail(string body, string to, string subject)
        {
            base.Name = "Send Email";
            this.body = body;   
            this.to = to;       
            this.subject = subject; 
        }

        public override void Execute()
        {
            string mailto = $"mailto:{this.to}?Subject={this.subject}&Body={body}";
            mailto = Uri.EscapeUriString(mailto);
            Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
        }

    }
}
