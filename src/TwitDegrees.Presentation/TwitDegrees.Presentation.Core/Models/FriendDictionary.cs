using System.Collections.Generic;
using System.Linq;
using Blackfin.Core.Collections;

namespace TwitDegrees.Presentation.Core.Models
{
    public class FriendDictionary
    {
        private readonly Dictionary<string, TwitterUser> friendsByName = new Dictionary<string, TwitterUser>();


        public FriendDictionary(IEnumerable<TwitterUser> users)
        {
            FriendsByCount = new ListDictionary<int, TwitterUser>();
            foreach (TwitterUser user in users)
            {
                friendsByName.Add(user.Name, user);
                FriendsByCount.Add(user.FriendCount, user);
            }
        }

        public ListDictionary<int, TwitterUser> FriendsByCount { get; private set; }

        public IEnumerable<TwitterUser> Friends
        {
            get 
            {
                return FriendsByCount
                    .FlattenList(new ReverseComparer<int>())
                    .Select(pair => pair.Value);
            }
        }

        public bool Contains(string user)
        {
            return friendsByName.ContainsKey(user);
        }
    }
}
