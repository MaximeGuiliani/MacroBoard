using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static MacroBoard.Utils;


namespace MacroBoard
{
    [Serializable]
    public class BlockMove : Block
    {
        public String source;
        public String destination;

        public BlockMove(String source, String destination)
        {
            base.Name = "Move Directory";
            base.info = "Move a folder from the specified source to the specified destination.";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockMoveFolder.png";

            this.source = source;
            this.destination = destination;
            base.category = Categories.Files;

        }


        public override void Execute()
        {
            string sourceDirectory = source;
            string destinationDirectory = destination;
            Directory.Move(sourceDirectory, concatPathWithFileName(destinationDirectory, Path.GetFileName(sourceDirectory)));
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
