using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MacroBoard
{
    [Serializable]
    public class BlockDeleteFile : Block
    {
        public String path;
        public BlockDeleteFile(String path)
        {
            base.Name = "Delete File";
            base.info = "Deletes the specified file.";
            base.category = Categories.Files;
            this.path = path;
        }
        public override void Execute()
        {
            File.Delete(path);
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
