using Newtonsoft.Json;

namespace Ukor.Configuration
{
    public class CommandLineDetails
    {
        [JsonRequired]
        public string Executable { get; set; }

        [JsonProperty]
        public string Arguments { get; set; }

        [JsonProperty]
        public string WorkingFolder { get; set; }

        [JsonRequired]
        public bool WaitForExit { get; set; }
    }
}
