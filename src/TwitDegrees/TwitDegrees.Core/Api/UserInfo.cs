

using System;

namespace TwitDegrees.Core.Api
{
    [Serializable]
    public class UserInfo
    {
        private string name;
        private string location;

        public string Name
        {
            get { return name; }
            set
            {
                name = Truncate(value);
                if (value == null)
                {
                    name = null;
                }
                else if (value.Length > 254)
                {
                    name = value.Substring(0, 254);
                }
                else
                {
                    name = value;
                }
            }
        }

        
        public string Location
        {
            get { return location; }
            set { location = Truncate(value); }
        }

        private static string Truncate(string value)
        {
            if (value == null || value.Length < 254)
            {
                return value;
            }
            return value.Substring(0, 254);
        }

        public int FriendCount { get; set; }
        public int FollowerCount { get; set; }

        public override string ToString()
        {
            return string.Format("[UserInfo Name={0} Location={1} Friends={2} Followers={3}",
                                 Name,
                                 Location,
                                 FriendCount,
                                 FollowerCount
                );
        }
    }
}
