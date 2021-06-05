using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HARD.Config
{
    public class AutoD_Config
    {
        private static readonly Lazy<AutoD_Config> instance = new Lazy<AutoD_Config>(() => new AutoD_Config());
        public static AutoD_Config Instance
        {
            get { return instance.Value; }
        }

        private string ConfigPath;
        public void Load()
        {
            var json = new JsonSerializer();
            using (var reader = new StreamReader(ConfigPath))
            {
                using (var jr = new JsonTextReader(reader))
                {
                    ConfigData = json.Deserialize<List<Actions>>(jr);
                }
            }
        }

        public void Save()
        {
            var json = new JsonSerializer();
            using (var sw = new StreamWriter(ConfigPath))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    json.Serialize(writer, ConfigData);
                }
            }
        }

        public List<Actions> ConfigData;

        public AutoD_Config()
        {
            if (!Directory.Exists("./Config"))
            {
                Directory.CreateDirectory("./Config");
            }

            ConfigPath = $"./Config/AutoD_Config.json";

            if (!File.Exists(ConfigPath))
            {
                File.Create(ConfigPath);
            }

            ConfigData = new List<Actions>();
        }

        public enum ClickType
        {
            LeftClick,
            RightClick,
            DoubleClick,
            SendKeys
        }

        public class Actions
        {
            public string ActionName { get; set; }
            public List<Action> Action { get; set; }
            public Actions(string ActionName, List<Action> Action)
            {
                this.ActionName = ActionName;
                this.Action = Action;
            }
        }

        public class Action
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Text { get; set; }
            public int Interval { get; set; }
            public string Type { get; set; }
            public Action(int X, int Y, string Text, int Interval, string Type)
            {
                this.X = X;
                this.Y = Y;
                this.Text = Text;
                this.Interval = Interval;
                this.Type = Type;
            }
        }
    }

    public class Linker_Config
    {
        private static readonly Lazy<Linker_Config> instance = new Lazy<Linker_Config>(() => new Linker_Config());
        public static Linker_Config Instance
        {
            get { return instance.Value; }
        }

        private string ConfigPath;
        public void Load()
        {
            var json = new JsonSerializer();
            using (var reader = new StreamReader(ConfigPath))
            {
                using (var jr = new JsonTextReader(reader))
                {
                    ConfigData = json.Deserialize<List<DirInfo>>(jr);
                }
            }
        }

        public void Save()
        {
            var json = new JsonSerializer();
            using (var sw = new StreamWriter(ConfigPath))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    json.Serialize(writer, ConfigData);
                }
            }
        }

        public List<DirInfo> ConfigData;

        public Linker_Config()
        {
            if (!Directory.Exists("./Config"))
            {
                Directory.CreateDirectory("./Config");
            }

            ConfigPath = $"./Config/Linker_Config.json";

            if (!File.Exists(ConfigPath))
            {
                File.Create(ConfigPath);
            }

            ConfigData = new List<DirInfo>();
        }

        public class DirInfo
        {
            public string NickName { get; set; }
            public string DirPath { get; set; }
            public DirInfo(string NickName, string DirPath)
            {
                this.NickName = NickName;
                this.DirPath = DirPath;
            }
        }
    }
}
