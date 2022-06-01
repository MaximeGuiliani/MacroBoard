using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroBoard
{


    internal class Serialization
    {
        string jsonPath;
        public Serialization(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public void Serialize(WorkFlow wf)
        {
            using (StreamWriter sw = new StreamWriter(jsonPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, wf);
            }

        }

        public WorkFlow Deserialize()
        {
            JObject WFJson = JObject.Parse(File.ReadAllText(jsonPath));
            string WFName = (string)WFJson["workflowName"]!;
            string WFImgPath = (string)WFJson["imagePath"]!;
            JArray WFList = (JArray)WFJson["workflowList"]!;

            int WFListSize = WFList.Count;
            Collection<Block> Blocks = new Collection<Block>();

            for (int i = 0; i < WFListSize; i++)
            {
                Type BlockType = Type.GetType("MacroBoard." + (string)WFList[i]!["BlockType"]!)!;
                Blocks.Add((Block)WFList[i]!.ToObject(BlockType)!);
            }
            return new WorkFlow(WFImgPath, WFName, Blocks);
        }


        public static void DeleteFAV(string WorkFlowName)
        {
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + WorkFlowName + ".json");
        }

        public static void DeleteWF(string WorkFlowName)
        {
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + WorkFlowName + ".json");

        }


        public static List<WorkflowView> getFavsFromJson()
        {
            List<WorkflowView> FavWorkflows = new();

            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\");
            FileInfo[] files = info.GetFiles("*.json").OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + file.Name);
                FavWorkflows.Add(new(serialization.Deserialize()));
            }

            return FavWorkflows;
        }


        public static List<WorkflowView> getWorkFlowsFromJson()
        {
            List<WorkflowView> Workflows = new();

            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON");
            FileInfo[] files = info.GetFiles("*.json").OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + file.Name);
                Workflows.Add(new(serialization.Deserialize()));
            }
            return Workflows;
        }


        public static ObservableCollection<WorkflowView> getWorkFlowsFromJsonZ()
        {
            ObservableCollection<WorkflowView> Workflows = new();

            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON");
            FileInfo[] files = info.GetFiles("*.json").OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + file.Name);
                Workflows.Add(new(serialization.Deserialize()));
            }
            return Workflows;
        }


        public static ObservableCollection<WorkflowView> getFavsFromJsonZ()
        {
            ObservableCollection<WorkflowView> FavWorkflows = new();

            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\");
            FileInfo[] files = info.GetFiles("*.json").OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                Serialization serialization = new Serialization(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\FAVJSON\" + file.Name);
                FavWorkflows.Add(new(serialization.Deserialize()));
            }

            return FavWorkflows;
        }
        
    }

}
