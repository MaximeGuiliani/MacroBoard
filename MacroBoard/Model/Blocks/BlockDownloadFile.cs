using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MacroBoard
{
    internal class BlockDownloadFile : Block
    {
        public String address;
        public String fileName;

        public BlockDownloadFile(String address, string fileName)
        {
            base.Name = "DownloadFile";
            base.info = "Downloads the resource with the specified URI to a local file.";
            this.address = address;
            this.fileName = fileName;
        }

        public override void Execute()
        {
           WebClient wc = new WebClient();
            wc.DownloadFile(address,fileName);
        }

    }
}
