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

        string automationID;
        public BlockInvokeAutomationId(string automationID)
        {
            base.Name = "BlockInvokeAutomationId";
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


        // ATTENTION: Les MessagesBox.Show("") changent l'app de 1er plan => marche pas
        public override void Execute()
        {

            //MessageBox.Show($"titleeeeeee: {GetActiveWindowTitle()}");
            IntPtr hwnd = GetForegroundWindow();
            AutomationElement elt_app = AutomationElement.FromHandle(hwnd);
            //MessageBox.Show($"eltName: {elt_app.Current.Name}");

            AutomationElementCollection elements = elt_app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationID));
            foreach (AutomationElement elt in elements)
            {
                //MessageBox.Show($"btn: {elt.Current.Name}");
                InvokePattern pattern = elt.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                //MessageBox.Show($"{pattern}");
                pattern.Invoke();
                break;
            }
        }


        

    }
}
