using System.Xml.Serialization;

namespace Ukor.StaticResponses
{
    [XmlType("active-app")]
    public class QueryActiveAppResponse
    {
        [XmlElement("app")] public string App { get; set; } = "Roku";
    }
}
