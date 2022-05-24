using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;



namespace MacroBoard
{
    class Config
    {

        const string relPathConfigFile = "./Resources/config/config.json";


        public static string PathFromList(string configId)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            JArray? paths = (JArray?)JObject.Parse(File.ReadAllText(relPathConfigFile))[configId];
            foreach (string path in paths)
            {
                if (File.Exists(path) || new Regex("^[a-zA-Z0-9-_]+\\.exe$").IsMatch(path))
                {
                    return path.Replace("$USERNAME", Environment.GetEnvironmentVariable("USERNAME"));
                }
            }
            return "";
        }


        public static string PathFromString(string configId)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            return ((string)JObject.Parse(File.ReadAllText(relPathConfigFile))[configId]).Replace("$USERNAME", Environment.GetEnvironmentVariable("USERNAME"));
        }

        public static string String(string configId)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            return ((string)JObject.Parse(File.ReadAllText(relPathConfigFile))[configId]);
        }



    }
}
//TODO check la regex

