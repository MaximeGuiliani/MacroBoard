using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class BlockSendEmail : Block
    {
        public string body;
        public string to;
        public string subject;

        public BlockSendEmail(string body, string to, string subject)
        {
            base.info = "Preparation for an email send.";
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


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
