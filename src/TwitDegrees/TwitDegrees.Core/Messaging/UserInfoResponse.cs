using System;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class UserInfoResponse : TwitterResponse
    {
        public UserInfoResponse(UserInfo userInfo)
        {
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; private set; }

        public override string ToString()
        {
            return "UserInfoResponse: " + UserInfo;
        }
    }
}
