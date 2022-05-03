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
        WorkFlow wf;
        public Serialization(WorkFlow wf)
        {
            this.wf = wf;
        }


        public void Serialize(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, this.wf);
            }
        }


        public WorkFlow Deserialize(string filePath)
        {
            JObject WFjson = JObject.Parse(File.ReadAllText(filePath));
            string WFName = (string)WFjson["wf_name"]!;
            JArray WFList = (JArray)WFjson["wf_list"]!;
            int WFListSize = WFList.Count;
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < WFListSize; i++)
            {
                Type BlockType = Type.GetType("MacroBoard." + (string)WFList[i]!["BlockType"]!)!;
                blocks.Add((Block)WFList[i]!.ToObject(BlockType)!);
            }
            return new WorkFlow("", WFName, blocks);
        }









    }

}
