using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MacroBoard
{
    internal class BlockWait : Block
    {
        private int hour, min, sec, sum;
        public BlockWait(int hour, int min, int sec)
        {
            base.Name = "Wait";
            this.hour = hour;
            this.min  = min;
            this.sec  = sec;
        }

        public override void Execute()
        {
            sum = sec * 1_000 + min * 60_000 + hour * 3_600_000;
            Thread.Sleep(sum);
        }



    }
}
