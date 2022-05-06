using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace MacroBoard
{
    internal class BlockCloseDesiredApplication : Block
    {
        public String appName { get; set; } = "";
        public BlockCloseDesiredApplication(String appName)
        {
            base.Name = "CloseDesiredApplication";
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

       
    }

}
