using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class WorkFlow
    {
        public string destinationPath = @"~\Documents\MacroBoard";
        public string imagePath { get; set; }
        public string workflowName { get; set; }
        public List<Block> workflowList { get; set; }

        public WorkFlow(string imagePath, string workflowName, List<Block> workflowList)
        {
            saveImage();
            this.imagePath = imagePath;
            this.workflowName = workflowName;
            this.workflowList = workflowList;
        }


        private void saveImage()
        {
            string extension = Path.GetExtension(imagePath);
            File.Copy(imagePath, @$"{this.destinationPath}\{workflowName}{extension}");
        }


    }
}
