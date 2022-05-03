using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace MacroBoard
{
    internal class CloseDesiredApplication : Block
    {
        String appName;
        public CloseDesiredApplication(String appName)
        {
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
