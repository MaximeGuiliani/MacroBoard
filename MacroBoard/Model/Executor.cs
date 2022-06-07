using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard.Model
{
    public class Executor
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
                    return "Error in execution of  " + block.GetType().Name+ "\n\n" + e.Message;
                }
            return ("");
        }



    }
}
