using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using static MacroBoard.Utils;

namespace MacroBoard
{
    [Serializable]
    public class BlockCloseDesiredApplication : Block
    {
        public String appName;
        public bool exactMatch;
        public bool kill;

        public BlockCloseDesiredApplication(String appName, bool exactMatch, bool kill)
        {
            base.Name       = "Close Application";
            base.LogoUrl    = "/Resources/Logo_Blocks/Logo_BlockCloseDesiredApplication.png";
            base.info       = "Close desired application";
            base.category   = Categories.Applications;
            this.appName    = appName;
            this.exactMatch = exactMatch;
            this.kill       = kill;
        }


        public override void Execute()
        {
            List<Process> processes = getProcesses(appName, exactMatch);
            foreach (Process process in processes)
            {
                try
                {
                    if (kill)
                        process.Kill();
                    else
                        process.CloseMainWindow();
                }
                catch (Exception e) {}
            }
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }




    }

}
