using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace MacroBoard
{
    internal class InvokeAutomationId : Block
    {
        string automationID;
        public InvokeAutomationId(string automationID)
        {
            this.automationID = automationID;   
        }
        public override void Execute()
        {
            [DllImport("user32.dll")]
            static extern IntPtr GetActiveWindow();
            AutomationElement elt_app = AutomationElement.FromHandle(GetActiveWindow());
            AutomationElementCollection automationElement =  elt_app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationID));
            foreach(AutomationElement ele in automationElement)
            {
                InvokePattern inv = ele.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                inv.Invoke();
                break;
            }
        }
    }
}
