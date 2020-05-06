using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ukor.Configuration
{
    [XmlRoot("apps", Namespace = "foo")]
    public class ApplicationList : List<Application>
    {
        public ApplicationList(Application[] applications) : base(applications){}
    }
}
