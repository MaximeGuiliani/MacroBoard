using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MacroBoard
{
    [Serializable]
    public class BlockWait : Block
    {
        public int hour, min, sec, sum, mili;
        public BlockWait(int mili, int sec = 0, int min = 0, int hour = 0)
        {
            base.info = "Wait the specified time.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockWait.png";
            base.Name = "Wait";
            this.mili = mili;
            this.sec = sec;
            this.min = min;
            this.hour = hour;
            base.category = Categories.System;

        }

        public override void Execute()
        {
            sum = mili + sec * 1_000 + min * 60_000 + hour * 3_600_000;
            //Thread.Sleep(sum);
            Task.Delay(sum).Wait();

        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
