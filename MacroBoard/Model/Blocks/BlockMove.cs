using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MacroBoard
{
    internal class BlockMove : Block
    {
        String source;
        String destination;
        public BlockMove(String source, String destination)
        {
            base.Name = "Move Folder/File"
            this.source = source;
            this.destination = destination;
        }


        public override void Execute()
        {
            string sourceDirectory = source;
            string destinationDirectory = destination;
            Directory.Move(sourceDirectory, destinationDirectory);
        }



    }
}
