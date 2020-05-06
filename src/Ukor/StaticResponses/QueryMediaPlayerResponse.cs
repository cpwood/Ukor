using System.Xml.Serialization;

namespace Ukor.StaticResponses
{
    [XmlType("player")]
    public class QueryMediaPlayerResponse
    {
        [XmlAttribute("error")]
        public bool Error { get; set; }

        [XmlAttribute("state")] public string State { get; set; } = "close";

        [XmlElement("format")]
        public QueryMediaPlayerResponseFormat Format { get; set; } = new QueryMediaPlayerResponseFormat();

        [XmlElement("is_live")]
        public bool IsLive { get; set; }
    }

    public class QueryMediaPlayerResponseFormat
    {
        [XmlAttribute("audio")] public string Audio { get; set; } = "aac_adts";
        [XmlAttribute("captions")] public string Captions { get; set; } = "none";
        [XmlAttribute("drm")] public string Drm { get; set; } = "none";
        [XmlAttribute("video")] public string Video { get; set; } = "mpeg4_10b";
    }
}
