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
    public class BlockMoveFile : Block
    {
        public String source;
        public String destination;

        public BlockMoveFile(String source, String destination)
        {
            base.Name = "Move File";
            base.info = "Move the specified file to the specified destination.";
            this.source = source;
            this.destination = destination;
        }


        public override void Execute()
        {
            Directory.Move(source, concatPathWithFileName(destination, Path.GetFileName(source)));
        }


        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
