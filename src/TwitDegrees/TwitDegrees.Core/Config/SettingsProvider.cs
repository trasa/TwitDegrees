using System.Configuration;

namespace TwitDegrees.Core.Config
{
    public interface ISettingsProvider
    {
        TwitterSection Twitter { get; }
    }
    public class SettingsProvider : ISettingsProvider
    {
        private static readonly TwitterSection twitter = (TwitterSection)ConfigurationManager.GetSection("twitter") ?? new TwitterSection();

        public TwitterSection Twitter
        {
            get { return twitter; }
        }

    }

    // for testing
    public class StubSettingsProvider : ISettingsProvider
    {
        public StubSettingsProvider(){}

        public StubSettingsProvider(TwitterSection twitter)
        {
            Twitter = twitter;
        }

        public TwitterSection Twitter
        {
            get; set;
        }
    }
}
