using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace MacroBoard 
{
    internal class Hibernate : Block
    {
        public override void Execute()
        {
            [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
            SetSuspendState(false, true, true);
        }
    }
}
