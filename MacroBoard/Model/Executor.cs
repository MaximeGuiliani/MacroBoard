using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard.Model
{
    internal class Executor
    {
        WorkFlow workFlow;

        public Executor(WorkFlow workFlow)
        {
            this.workFlow = workFlow;
        }

       public string Execute()
        {
            foreach (Block block in workFlow.workflowList)
                try
                {
                    block.Execute();                 
                }
                catch(Exception e)
                {
                    return block.GetType().Name+ " " + e.Message;
                }
            return ("");
        }
    }
}
