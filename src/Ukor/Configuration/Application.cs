using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace Ukor.Configuration
{
    [XmlType("app")]
    public class Application
    {
        public enum ActionType
        {
            None,
            CSharp,
            CommandLine
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum KeyPress
        {
            Home,
            Rev,
            Fwd,
            Play,
            Select,
            Left,
            Right,
            Down,
            Up,
            Back,
            InstantReplay,
            Info,
            Backspace,
            Search,
            Enter
        }

        [XmlAttribute("id")]
        [JsonRequired, JsonProperty("Id")]
        public int Id { get; set; }

        [XmlText]
        [JsonRequired]
        public string Name { get; set; }

        [XmlIgnore]
        [JsonProperty]
        public string Description { get; set; }

        [XmlIgnore]
        [JsonRequired, JsonConverter(typeof(StringEnumConverter))]
        public ActionType Action { get; set; }

        [XmlIgnore]
        [JsonProperty]
        public CommandLineDetails CommandLineDetails { get; set; }

        [XmlIgnore]
        [JsonProperty]
        public CSharpDetails CSharpDetails { get; set; }
        
        [XmlAttribute("subtype")] 
        [JsonIgnore]
        public string SubType { get; set; } = "rsga";

        [XmlAttribute("type")] 
        [JsonIgnore]
        public string Type { get; set; } = "appl";

        [XmlAttribute("version")] 
        [JsonIgnore] 
        public string Version { get; set; } = "1.0.0";

        [XmlIgnore]
        [JsonProperty]
        public KeyPress[] LaunchKeySequence { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        internal ICSharpAction ActionClass { get; set; }
    }
}
