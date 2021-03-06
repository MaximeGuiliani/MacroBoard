using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroBoard
{
    public class WorkFlow
    {
        public string destinationPath = $@"C:\Users{Environment.GetEnvironmentVariable("USERNAME")}\Documents";
        public string imagePath { get; set; }
        public string workflowName { get; set; }
        public Collection<Block> workflowList { get; set; }

        public WorkFlow(string imagePath, string workflowName, Collection<Block> workflowList)
        {
            //saveImage();
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
