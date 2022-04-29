using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class WorkFlow
    {
        public string imagePath { get; set; }
        public string workflowName { get; set; }
        public List<Block> workflowList { get; set; }

        public WorkFlow(string imagePath, string workflowName, List<Block> workflowList)
        {
            this.imagePath = imagePath;
            this.workflowName = workflowName;
            this.workflowList = workflowList;
        }

    }
}
