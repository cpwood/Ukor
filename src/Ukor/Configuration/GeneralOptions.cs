using Newtonsoft.Json;

namespace Ukor.Configuration
{
    public class GeneralOptions
    {
        [JsonProperty] 
        public int KeySequenceLength { get; set; } = 2;
    }
}
