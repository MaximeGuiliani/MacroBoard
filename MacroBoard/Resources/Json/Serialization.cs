using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            List<Block> Blocks = new List<Block>();

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

    }

}
