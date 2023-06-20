using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Private_Apex.Core
{
    public class Config
    {
        private ConfigStructure _config { get; set; }
        private string _path { get; init; }

        public Config(string path)
        {
            _path = path;
            _config = !File.Exists(path) ? new ConfigStructure() : JsonConvert.DeserializeObject<ConfigStructure>(File.ReadAllText(path));
        }

        public int? GetLangID() => _config.LanguageID;
        public void SetLangID(int? langId) => _config.LanguageID = langId;
        public void Save() => File.WriteAllText(_path, JsonConvert.SerializeObject(_config));
    }
    public class ConfigStructure
    {
        [JsonProperty("LanguageID")]
        public int? LanguageID { get; set; }
    }
}
