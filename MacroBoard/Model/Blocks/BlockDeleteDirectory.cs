﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MacroBoard
{
    public class BlockDeleteDirectory : Block
    {
        public String path;
        public BlockDeleteDirectory(String path)
        {
            base.Name = "Delete Directory";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockDeleteDirectory.png";
            base.info = "Deletes the specified directory.";
            this.path = path;   
        }
        public override void Execute()
        {
          Directory.Delete(path, true);
        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
