using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MacroBoard
{
    internal class DeleteDirectory : Block
    {
        String path;
        public DeleteDirectory(String path)
        {
            this.path = path;   
        }
        public override void Execute()
        {
          Directory.Delete(path, true);
        }


    }
}
