using System;
using System.Net;
using System.Reflection;
using Blackfin.Core.NUnit;
using log4net;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.TwitterClientFixtures
{
    [TestFixture]
    public class Error_Handler : ContextSpecification
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [Test]
        public void When_Gateway_Timeout_Once()
        {
            const string commResult = "heres a message";
            var comm = new CommFailsFirstTime
                           {
                               Exception = new TwitterException("(504) Timeout") {StatusCode = HttpStatusCode.GatewayTimeout},
                               Result = commResult,
                           };
            var clientUnderTest = new TwitterClient(comm);
            string result = clientUnderTest.GetUser("test");
            Assert.That(result, Is.EqualTo(commResult));
            Assert.That(comm.ThrewException, Is.True);
        }

        [Test]
        public void When_Gateway_Timeout_Too_Many_Times()
        {
            var comm = new Mock<ITwitterComm>();
            const int expectedTries = 2;
            comm.Setup(c => c.ExecuteGet(It.IsAny<string>())).Throws(new TwitterException("timeout") {StatusCode = HttpStatusCode.GatewayTimeout});
            var clientUnderTest = new TwitterClient(comm.Object) {MaxCommunicationTries = expectedTries};
            
            Expect.Exception<TwitterException>(() => clientUnderTest.GetUser("test"));
            comm.Verify(c => c.ExecuteGet(It.IsAny<string>()), Times.Exactly(expectedTries));
        }


        class CommFailsFirstTime : ITwitterComm
        {
            private bool firstTime = true;

            public Exception Exception { get; set; }
            public string Result { get; set; }

            public bool ThrewException
            {
                get{ return !firstTime;}
            }

            public string ExecuteGet(string url)
            {
                if (firstTime)
                {
                    firstTime = false;
                    throw Exception;
                }
                return Result;
            }
        }
    }
}
