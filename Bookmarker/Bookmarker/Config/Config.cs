using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bookmarker
{
    public class ConfigStruct
    {
        public bool isChrome { get; set; }
        public string nickname { get; set; }
        public string path { get; set; }

        public ConfigStruct(bool isChrome, string nickname, string path)
        {
            this.isChrome = isChrome;
            this.nickname = nickname;
            this.path = path;
        }
    }

    public class Config
    {
        private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
        
        private string _configPath;

        private Config()
        {
            _configPath = $"./Config/config.json";

            CheckFile();

            Load();
        }

        public static Config Instance
        {
            get { return _instance.Value; }
        }

        private List<ConfigStruct> _configData = new List<ConfigStruct>();

        private void CheckFile()
        {
            if (File.Exists(_configPath)) return;

            _configData.Add(new ConfigStruct(false, "Default", "D:\\"));

            Save();
        }

        public List<ConfigStruct> ConfigData => _configData;

        public void Load()
        {
            if (!File.Exists(_configPath)) return;

            var json = new JsonSerializer();

            using (var sr = new StreamReader(_configPath))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    _configData = json.Deserialize<List<ConfigStruct>>(jr);
                }
            }
        }

        public void Save()
        {
            var json = new JsonSerializer();

            using (var sw = new StreamWriter(_configPath))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    json.Serialize(jw, _configData);
                }
            }
        }
    }
}
