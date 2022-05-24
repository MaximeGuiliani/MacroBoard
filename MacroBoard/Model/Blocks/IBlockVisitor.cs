using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public interface IBlockVisitor
    {
        public void Visit(BlockClickL b);
        public void Visit(BlockClickR b);
        public void Visit(BlockCloseDesiredApplication b);
        public void Visit(BlockCopy b);
        public void Visit(BlockCreateTextFile b);
        public void Visit(BlockDeleteDirectory b);
        public void Visit(BlockDownloadFile b);
        public void Visit(BlockHibernate b);
        public void Visit(BlockInvokeAutomationId b);
        public void Visit(BlockKeyBoard b);
        public void Visit(BlockLaunchApp b);
        public void Visit(BlockLaunchBrowserChrome b);
        public void Visit(BlockLaunchBrowserEdge b);
        public void Visit(BlockLaunchBrowserFirefox b);
        public void Visit(BlockLock b);
        public void Visit(BlockMessageBox b);
        public void Visit(BlockMove b);
        public void Visit(BlockRecognition b);
        public void Visit(BlockRestart b);
        public void Visit(BlockRunScript b);
        public void Visit(BlockScreenshot b);
        public void Visit(BlockSendEmail b);
        public void Visit(BlockSetCursor b);
        public void Visit(BlockShutdown b);
        public void Visit(BlockWait b);
        public void Visit(BlockWindowStyle b);
        public void Visit(BlockCopyFile b);
        public void Visit(BlockDeleteFile b);
        public void Visit(BlockMoveFile b);

    }
}
