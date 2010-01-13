using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Test.FriendCrawlerFixtures
{
    [TestFixture]
    public class When_Crawling_UserInfoRequest : ContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            StatusService = new Mock<IStatusService>();
            StatusService.Setup(ss => ss.GetUser("trasa")).Returns(new UserInfo
                                                                       {
                                                                           Name = "trasa", 
                                                                           FollowerCount = 1, 
                                                                           FriendCount = 1,
                                                                           Location = "location",
                                                                       });
            RequestQueue = new Mock<ITwitterRequestQueue>();
            ResponseQueue = new TwitterResponseQueueStub();
            CrawlerUnderTest = new TwitterCrawler(RequestQueue.Object, ResponseQueue, StatusService.Object);
        }

       

        protected TwitterCrawler CrawlerUnderTest { get; set; }
        protected Mock<IStatusService> StatusService { get; set; }
        protected Mock<ITwitterRequestQueue> RequestQueue { get; set; }
        protected TwitterResponseQueueStub ResponseQueue { get; set; }


        [Test]
        public void ProcessUserInfoRequest()
        {
            CrawlerUnderTest.ProcessUserInfoRequest(new UserInfoRequest("trasa"));

            StatusService.Verify(ss => ss.GetUser("trasa"), Times.Once());
            Assert.That(ResponseQueue.SentResponses.Count, Is.EqualTo(1));
            var responseUserInfo = ((UserInfoResponse)ResponseQueue.SentResponses.Single()).UserInfo;
            Assert.That(responseUserInfo.Name, Is.EqualTo("trasa"));
            Assert.That(responseUserInfo.FollowerCount, Is.EqualTo(1));
            Assert.That(responseUserInfo.FriendCount, Is.EqualTo(1));
            Assert.That(responseUserInfo.Location, Is.EqualTo("location"));
        }

        [Test]
        public void ProcessRequest()
        {
            CrawlerUnderTest.ProcessRequest(new UserInfoRequest("trasa"));

            StatusService.Verify(ss => ss.GetUser("trasa"), Times.Once());
            Assert.That(ResponseQueue.SentResponses.Count, Is.EqualTo(1));
            var responseUserInfo = ((UserInfoResponse)ResponseQueue.SentResponses.Single()).UserInfo;
            Assert.That(responseUserInfo.Name, Is.EqualTo("trasa"));
            Assert.That(responseUserInfo.FollowerCount, Is.EqualTo(1));
            Assert.That(responseUserInfo.FriendCount, Is.EqualTo(1));
            Assert.That(responseUserInfo.Location, Is.EqualTo("location"));
        }
    }
}
