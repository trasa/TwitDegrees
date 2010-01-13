using System;
using System.Collections.Generic;
using System.Linq;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class GetFriendsResponse : TwitterResponse
    {
        
        public GetFriendsResponse(string userName, IEnumerable<UserInfo> friendsOfUser)
        {
            UserName = userName;
            FriendsOfUser = friendsOfUser.ToArray();
        }

        public string UserName { get; private set; }
        public UserInfo[] FriendsOfUser { get; private set; }

        public override string ToString()
        {
            return "GetFriendsResponse: " + UserName + " has " + FriendsOfUser.Length + " friends";
        }
    }
}
