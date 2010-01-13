using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Repositories;
using TwitDegrees.Core.Services;
using TwitDegrees.Test.Messaging;

namespace TwitDegrees.Test.CrawlerControllerFixtures
{
    [TestFixture]
    public class When_Processing_ErrorResponse
    {
        [Test]
        public void Request_Is_Retried()
        {
            // arrange
            var userWriter = new Mock<IUserWriter>();
            var requestQueue = new MockRequestQueue();
            var responseQueue = new MockResponseQueue();
            var controller = new CrawlerController(userWriter.Object, requestQueue, responseQueue, new RecentFriendRegistry());
            var retryRequest = new GetFriendsRequest("test");
            var errorResponse = new ErrorResponse(retryRequest, "fail whale");

            // act
            controller.ProcessResponse(errorResponse);

            // assert
            Assert.That(requestQueue.QueuedMessages, Has.Count(1));
            Assert.That(requestQueue.QueuedMessages.Single(), Is.SameAs(retryRequest));
        }

        [Test, Ignore("can't change until we have empty queues")]
        public void Request_Is_Failed_If_Too_Many_Retries()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
