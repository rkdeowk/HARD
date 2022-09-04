using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reference.Config
{
    class JsonHandler
    {
        private static readonly Lazy<JsonHandler> instance = new Lazy<JsonHandler>(() => new JsonHandler());
        public static JsonHandler Instance { get { return instance.Value; } }

        public class ConfigInformation
        {
            public string _name;
            public List<MemberList> _memberList;
            public ConfigInformation(string name, List<MemberList> memberList)
            {
                this._name = name;
                this._memberList = memberList;
            }
        }

        public class MemberList
        {
            public string _name;
            public int _age;
            public MemberList(string name, int age)
            {
                this._name = name;
                this._age = age;
            }
        }

        private readonly string _defaultJsonFilePath = $@"{Environment.CurrentDirectory}\Config\config.json";
        private ConfigInformation _configInformation;

        private JsonHandler()
        {
            Load();
        }

        public void Load(string filePath = "")
        {
            filePath = string.IsNullOrWhiteSpace(filePath) ? this._defaultJsonFilePath : filePath;

            if (!File.Exists(filePath)) return;

            this._configInformation = JsonConvert.DeserializeObject<ConfigInformation>(File.ReadAllText(filePath));
        }

        public void Save(string filePath = "")
        {
            filePath = string.IsNullOrWhiteSpace(filePath) ? this._defaultJsonFilePath : filePath;

            using (StreamWriter sw = File.CreateText($"{filePath}"))
            {
                sw.WriteLine(JsonConvert.SerializeObject(this._configInformation, Formatting.Indented));
            }
        }

        public ConfigInformation GetConfig()
        {
            return this._configInformation;
        }
    }
}
