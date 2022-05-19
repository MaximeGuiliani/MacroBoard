using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MacroBoard.Utils;

namespace MacroBoard
{
    public class BlockCreateTextFile : Block
    {
        public string filePath, fileName, text;
        public BlockCreateTextFile(string filePath, string fileName, string text)
        {
            base.Name = "Create text file";
            base.LogoUrl = "/Resources/Logo_Blocks/Logo_BlockCreateTextFile.png";
            base.info = "Create a text file of any type";
            this.filePath = filePath;
            this.fileName = fileName;
            this.text = text;
            base.category = Categories.Files;

        }

        public override void Execute()
        {
            
            Process.Start("powershell.exe", "ADD-content -path " +
                "'"+ concatPathWithFileName(filePath, fileName) + "'" +
                " -value " + "'" + text + "'");

        }

        public override void Accept(IBlockVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}
