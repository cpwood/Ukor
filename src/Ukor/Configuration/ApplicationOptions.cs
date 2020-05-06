using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ukor.Configuration
{
    public class ApplicationOptions
    {
        [JsonProperty("Applications")]
        public Application[] Applications { get; set; }

        [JsonIgnore]
        public ApplicationList List => new ApplicationList(Applications);
    }
}
