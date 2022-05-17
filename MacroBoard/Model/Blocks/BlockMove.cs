using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MacroBoard
{
    public class BlockMove : Block
    {
        public String source;
        public String destination;
        public BlockMove(String source, String destination)
        {
            base.Name = "Move Folder/File";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockMoveFile.png";
            base.info = "Move a folder/file from the specified source to the specified destination.";
            this.source = source;
            this.destination = destination;
        }


        public override void Execute()
        {
            string sourceDirectory = source;
            string destinationDirectory = destination;
            Directory.Move(sourceDirectory, destinationDirectory);
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
