using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class UserInfoRequest : TwitterRequest
    {
        public UserInfoRequest(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }

        public override string ToString()
        {
            return "UserInfoRequest: " + UserName;
        }
    }
}
