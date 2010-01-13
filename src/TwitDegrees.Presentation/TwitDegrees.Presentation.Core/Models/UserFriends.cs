using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitDegrees.Presentation.Core.Models
{
    public class UserFriends
    {
        public virtual string UserName { get; set; }
        public virtual TwitterUser IsFriendOf { get; set; }
        public virtual string IsFriendOfName { get; set;}


        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var lhs = (UserFriends)obj;
            return Equals(UserName, lhs.UserName) && Equals(IsFriendOf, lhs.IsFriendOf);
        }

        public override int GetHashCode()
        {
            return UserName.GetHashCode();
        }
    }
}
