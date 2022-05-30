using System;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using static MacroBoard.Utils;

namespace MacroBoard
{
    [Serializable]
    public class BlockInvokeAutomationId : Block
    {

        public string automationID;


        public BlockInvokeAutomationId(string automationID)
        {
            base.Name = "Invoke AutomationId";
            base.info = "Use the Invoke Pattern on the UIElement from the foreground window.\nTips: Look at the Pattern Support";
            base.category = Categories.Applications;
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockInvoke.png";
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
                if (pattern == null) continue;
                pattern.Invoke();
                break; // <=> on invoque un seul sur les trouvés
            }
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
