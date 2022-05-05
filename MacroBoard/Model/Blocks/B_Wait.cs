using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MacroBoard.Blocks
{
    internal class B_Wait : Block
    {
        private int hour, min, sec, sum;
        public B_Wait(string Name, string LogoUrl, string info, int hour, int min, int sec)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;

            this.hour = hour;
            this.min = min;
            this.sec = sec;
        }

        public override void Execute()
        {
            sum = sec * 1_000 + min * 60_000 + hour * 3_600_000;
            Thread.Sleep(sum);
        }



    }
}
