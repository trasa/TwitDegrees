using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TwitDegrees.Core.Api
{
    public interface IStatusService
    {
        IEnumerable<UserInfo> GetFriendsOf(string userName);
        UserInfo GetUser(string userName);
        IEnumerable<UserInfo> GetFollowersOf(string userName);
    }

    public class StatusService : IStatusService
    {
        private readonly ITwitterClient twitterClient;
        private readonly IRateLimitStatusProvider rateLimitStatusProvider;

        public StatusService(ITwitterClient twitterClient, IRateLimitStatusProvider rateLimitStatusProvider)
        {
            this.twitterClient = twitterClient;
            this.rateLimitStatusProvider = rateLimitStatusProvider;
            MaxCommunicationTries = 5;
        }

        public int MaxCommunicationTries { get; set; }

        public IEnumerable<UserInfo> GetFriendsOf(string userName)
        {
            return GetPagedResults(page => twitterClient.GetFriends(userName, page));
        }

        public IEnumerable<UserInfo> GetFollowersOf(string userName)
        {
            return GetPagedResults(page => twitterClient.GetFollowers(userName, page));
        }

        private IEnumerable<UserInfo> GetPagedResults(Func<int, string> pagedTwitterMethod)
        {
            int page = 1;
            var result = new List<UserInfo>();
            while (true)
            {
                int retryPage = page;
                IEnumerable<UserInfo> users = new TwitterRetry(MaxCommunicationTries).Try(
                    () =>
                        {
                            string xml = pagedTwitterMethod(retryPage);
                            if (string.IsNullOrEmpty(xml))
                            {
                                // user doesn't exist, has been disabled, is hidden, or otherwise unreachable.
                                // oh well.
                                return new UserInfo[0];
                            } 
                            return XDocument
                                .Parse(xml)
                                .Descendants("user")
                                .Select(user => BuildUserInfo(user));
                        }
                    );
                result.AddRange(users);
                if (users.Count() == 0)
                {
                    // there aren't any more.
                    break;
                }
                // get another page and add it to the result
                page++;
            }
            return result;
        }





        public UserInfo GetUser(string userName)
        {
            string userInfo = twitterClient.GetUser(userName);
            if (userInfo == null)
            {
                // for some reason we couldn't get the user, maybe they've protected their updates
                // c'est la vie.
                return null;
            }
            return BuildUserInfo(XDocument.Parse(userInfo).Root);
        }



        private static UserInfo BuildUserInfo(XContainer user)
        {
            // ReSharper disable PossibleNullReferenceException
            return new UserInfo
                       {
                           Name = user.Element("screen_name").Value,
                           Location = user.Element("location").Value,
                           FriendCount = int.Parse(user.Element("friends_count").Value),
                           FollowerCount = int.Parse(user.Element("followers_count").Value)
                       };
            // ReSharper restore PossibleNullReferenceException
        }


        public RateLimitStatus GetRateLimitStatus()
        {
            return rateLimitStatusProvider.GetRateLimitStatus();
        }
    }
}
