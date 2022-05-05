using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    internal class SendEmail : Block
    {
        string body;
        string to;
        string subject;
        public SendEmail(string body, string to, string subject)
        {
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
