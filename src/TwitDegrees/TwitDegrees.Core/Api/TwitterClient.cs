
using System.Reflection;
using log4net;

namespace TwitDegrees.Core.Api
{
    public interface ITwitterClient
    {
        string GetFriends(string userName, int page);
        string GetFollowers(string userName, int page);
        string GetUser(string userName);
    }

    public class TwitterClient : ITwitterClient
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITwitterComm twitterComm;

        public TwitterClient(ITwitterComm twitterComm)
        {
            this.twitterComm = twitterComm;
            MaxCommunicationTries = 5;
        }

        public int MaxCommunicationTries { get; set; }

        public string GetFriends(string userName, int page)
        {
            return ExecuteGet("/statuses/friends/" + userName + ".xml?page=" + page);
        }

        public string GetFollowers(string userName, int page)
        {
            return ExecuteGet("/statuses/followers/" + userName + ".xml?page=" + page);
        }

        public string GetUser(string userName)
        {
            return ExecuteGet("/users/show/" + userName + ".xml");
        }

        protected virtual string ExecuteGet(string url)
        {
            return (new TwitterRetry(MaxCommunicationTries).Try(() => twitterComm.ExecuteGet(url)));
        }
    }
}
