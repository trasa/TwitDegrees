using System.Linq;
using System.Xml;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Services;
using TwitDegrees.Test.Messaging;

namespace TwitDegrees.Test.FriendCrawlerFixtures
{
    [TestFixture]
    public class When_Processing_Request_Generates_Exception 
    {

        [Test]
        public void ErrorResponse_Is_Queued()
        {
            // arrange
            var requestQueue = new MockRequestQueue();
            var responseQueue = new MockResponseQueue();
            var statusService = new Mock<IStatusService>();

            statusService.Setup(ss => ss.GetFriendsOf("test")).Throws(new XmlException("Twitter is full of FAIL!"));
            var crawler = new TwitterCrawler(requestQueue, responseQueue, statusService.Object);

            // act
            var friendRequest = new GetFriendsRequest("test");
            crawler.ProcessRequest(friendRequest);

            // assert
            Assert.That(responseQueue.QueuedMessages, Has.Count(1));
            Assert.That(responseQueue.QueuedMessages.Single(), Is.TypeOf(typeof(ErrorResponse)));
            var response = (ErrorResponse)responseQueue.QueuedMessages.Single();
            Assert.That(response.Request, Is.SameAs(friendRequest));
        }
    }
}
