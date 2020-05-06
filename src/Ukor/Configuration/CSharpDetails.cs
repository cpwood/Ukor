using Newtonsoft.Json;

namespace Ukor.Configuration
{
    public class CSharpDetails
    {
        [JsonRequired]
        public string AssemblyPath { get; set; }

        [JsonRequired]
        public string ClassName { get; set; }
    }
}
