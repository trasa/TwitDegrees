using System;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class GetFriendsRequest : TwitterRequest
    {
        public GetFriendsRequest(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }

        public override string ToString()
        {
            return "GetFriendsRequest: " + UserName;
        }
    }
}
