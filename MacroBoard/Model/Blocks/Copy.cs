using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MacroBoard
{
    public class Copy : Block
    {
        String source;
        String destination;
        public Copy(String source, String destination)
        {
            this.source = source;
            this.destination = destination;
        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);    
        }

        public override void Execute()
        {
            FileAttributes attr = File.GetAttributes(source);

            if (attr.HasFlag(FileAttributes.Directory)) {
                CopyDirectory(source, destination, true);
            }

            else
            {
                FileInfo file = new FileInfo(source);
                file.CopyTo(destination + ((source.EndsWith(@"\")) ? "" : @"\") + Path.GetFileName(source)) ;
            }
                
        }
    }
}
