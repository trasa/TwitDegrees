using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TwitDegrees.GraphViewer.Infrastructure.Model
{
    [DataContract(Namespace = "TwitDegrees.Web.Api.Data")]
    public class TwitterUser
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public TwitterUser[] Friends { get; set; }

    }
}
