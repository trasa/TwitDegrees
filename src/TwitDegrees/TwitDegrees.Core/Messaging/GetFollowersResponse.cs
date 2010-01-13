using System;
using System.Collections.Generic;
using System.Linq;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class GetFollowersResponse : TwitterResponse
    {
        public string UserName { get; private set; }
        public UserInfo[] FollowersOfUser { get; private set; }

        public GetFollowersResponse(string userName, IEnumerable<UserInfo> followersOfUser)
        {
            UserName = userName;
            FollowersOfUser = followersOfUser.ToArray();
        }

        public override string ToString()
        {
            return "GetFollowersResponse: " + UserName + " has " + FollowersOfUser.Length + " followers";
        }
    }
}
