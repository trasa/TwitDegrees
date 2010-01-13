using System.Reflection;
using log4net;
using NUnit.Framework;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Test.Services
{
    [TestFixture]
    public class RecentFriendRegistryFixture
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void When_Crawl_Disabled_Always_Deny_Requests()
        {
            var registryUnderTest = new RecentFriendRegistry();
            const string user = "test";
            Assert.IsFalse(registryUnderTest.ShouldCrawlThisUser(user));
            Assert.IsFalse(registryUnderTest.ShouldCrawlThisUser(user));
            Assert.IsFalse(registryUnderTest.ShouldCrawlThisUser(user));
        }

        [Test]
        public void When_Recently_Seen_Deny_Request()
        {
            var registryUnderTest = new RecentFriendRegistry { IsCrawlEnabled = true};
            const string user = "test";
            
            Assert.IsTrue(registryUnderTest.ShouldCrawlThisUser(user));
            
            registryUnderTest.RegisterRecentFriend(user);
            
            Assert.IsFalse(registryUnderTest.ShouldCrawlThisUser(user));
        }
    }
}
