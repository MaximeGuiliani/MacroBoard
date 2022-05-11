using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace MacroBoard
{
    internal class BlockInvokeAutomationId : Block
    {

        public string automationID;
        public BlockInvokeAutomationId(string automationID)
        {
            base.Name = "BlockInvokeAutomationId";
            base.info = "Locate and click the defined AutomationElement";
            this.automationID = automationID;
        }


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
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


        

    }
}
