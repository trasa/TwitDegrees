using Moq;
using NUnit.Framework;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Test.CrawlerControllerFixtures
{
    [TestFixture]
    public class Detect_Loops : ContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            User = "trasa";
            RequestQueue = new Mock<ITwitterRequestQueue>();
            var responseQueue = new Mock<ITwitterResponseQueue>();
            RecentFriendRegistry = new Mock<IRecentFriendRegistry>();
            ControllerUnderTest = new CrawlerController(null, RequestQueue.Object, responseQueue.Object, RecentFriendRegistry.Object);
        }

        protected string User { get; set; }
        protected CrawlerController ControllerUnderTest { get; set; }
        protected Mock<IRecentFriendRegistry> RecentFriendRegistry { get; set; }
        protected Mock<ITwitterRequestQueue> RequestQueue { get; set; }

        [Test]
        public void Avoid_Users_We_Have_Seen_Lately()
        {
            RecentFriendRegistry.Setup(r => r.ShouldCrawlThisUser(User)).Returns(false);
            ControllerUnderTest.CrawlFriendsOf(User);

            RequestQueue.Verify(q => q.Send(It.IsAny<TwitterRequest>()), Times.Never());
        }
    }
}
