using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace TwitDegrees.Core.Services
{
    public interface IRecentFriendRegistry
    {
        bool IsRecentFriend(string friend);
        void RegisterRecentFriend(string friend);
        bool ShouldCrawlThisUser(string friend); 
        bool IsCrawlEnabled { get; set; }
    }

    public class RecentFriendRegistry : IRecentFriendRegistry
    {
        private readonly Dictionary<string, string> recentFriends = new Dictionary<string, string>();
        private readonly object friendLock = new object();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        
        public bool IsCrawlEnabled { get; set; }

        public bool IsRecentFriend(string friend)
        {
            if (friend == null)
                throw new ArgumentNullException("friend");

            lock (friendLock)
            {
                return recentFriends.ContainsKey(friend);
            }
        }

        public void RegisterRecentFriend(string friend)
        {
            if (friend == null)
                throw new ArgumentNullException("friend");

            lock (friendLock)
            {
                string value;
                if (!recentFriends.TryGetValue(friend, out value))
                {
                    recentFriends.Add(friend, friend);
                }
            }
        }


        public bool ShouldCrawlThisUser(string friend)
        {
            if (!IsCrawlEnabled)
            {
//                log.Debug("Don't Crawl " + friend.Name + " - Crawling disabled by configuration.");
                return false;
            }
            if (IsRecentFriend(friend))
            {
                // we've seen this user in the current session
                log.Debug("Don't crawl " + friend + " - Recent Friend");
                return false;
            }
//
//          NOTE: we don't have access to LastUpdated data anymore, because we're using sprocs for updating and not
//          getting LastUpdated values back..trying to make the crawler faster.
//
//            TimeSpan lastScan = DateTime.Now - (friend.LastUpdated ?? DateTime.MinValue);
//            if (lastScan.TotalHours < lastScanThresholdHours)
//            {
//                // we've seen this user in the recent past
//                log.DebugFormat("Don't crawl {0} - last scan was {1:0.00} hours ago.", friend.Name, lastScan.TotalHours);
//                return false;
//            }
            return true;
        }
    }
}
