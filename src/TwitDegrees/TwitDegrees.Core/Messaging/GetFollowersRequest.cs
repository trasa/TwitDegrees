using System;


namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class GetFollowersRequest : TwitterRequest
    {
        public GetFollowersRequest(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }

        public override string ToString()
        {
            return "GetFollowersRequest: " + UserName;
        }
    }
}
