using System;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using static MacroBoard.Utils;

namespace MacroBoard
{
    public class BlockInvokeAutomationId : Block
    {

        public string automationID;


        public BlockInvokeAutomationId(string automationID)
        {
            base.Name = "BlockInvokeAutomationId";
            base.info = "Invoke the AutomationElement on the foreground window.";
            this.automationID = automationID;
        }


        public override void Execute()
        {
            IntPtr hwnd = GetForegroundWindow();
            AutomationElement elt_app = AutomationElement.FromHandle(hwnd);
            AutomationElementCollection elements = elt_app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationID));
            foreach (AutomationElement elt in elements)
            {
                InvokePattern pattern = elt.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                pattern.Invoke();
                break;
            }
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
