
using System.Collections.Generic;

namespace TwitDegrees.Presentation.Core.Models
{
    public class TwitterUser
    {
        public virtual string Name { get; set; }
        public virtual int FriendCount { get; set; }
        public virtual IList<UserFriends> Friends { get; set;}
    }
}
