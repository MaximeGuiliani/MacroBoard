using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MacroBoard
{
    internal class BlockDeleteDirectory : Block
    {
        public String path;
        public BlockDeleteDirectory(String path)
        {
            base.Name = "Delete Directory";
            base.info = "Deletes the specified directory.";
            this.path = path;   
        }
        public override void Execute()
        {
          Directory.Delete(path, true);
        }


    }
}
