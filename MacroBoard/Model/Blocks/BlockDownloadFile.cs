using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using static MacroBoard.Utils;

namespace MacroBoard
{
    [Serializable]
    public class BlockDownloadFile : Block
    {
        public String address;
        public String folderPath;

        public BlockDownloadFile(String address, string folderPath)
        {
            base.Name = "DownloadFile";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockDownload.png";
            base.info = "Downloads the resource with the specified URI to a local file.";
            this.address = address;
            this.folderPath = folderPath;
            base.category = Categories.Files;

        }

        public override void Execute()
        {
            WebClient wc = new WebClient();
            wc.DownloadFile( address, concatPathWithFileName(folderPath, Path.GetFileName(address)) );
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
