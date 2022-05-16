using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace MacroBoard
{
    public class BlockCloseDesiredApplication : Block
    {
        public String appName { get; set; } = "";
        public BlockCloseDesiredApplication(String appName)
        {
            base.Name = "CloseDesiredApplication";
            base.info = "Close desired application.";
            this.appName = appName;

        }
        public override void Execute()
        {
           Process[] processes = Process.GetProcessesByName(appName);
            foreach (Process process in processes)
            {
                process.CloseMainWindow();
            }
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }

}
