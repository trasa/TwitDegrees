using System.Threading;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Messaging;

namespace TwitDegrees.Test.Messaging
{
    [TestFixture]
    public class Send_Test_Message_To_Request_Queue : ContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            requestQueue = new TwitterRequestQueue(@"FormatName:DIRECT=OS:sqldev\private$\TwitterRequest");
        }

        protected TwitterRequestQueue requestQueue { get; set; }

        protected override void CleanUp()
        {
            base.CleanUp();
            if (requestQueue != null)
                requestQueue.Dispose();
        }

        [Test, Ignore("test fails when there are already a bunch of messages in the queue")]
        public void SendAndReceive()
        {
            var resetEvent = new ManualResetEvent(false);
            const string message = "hello, world";
            bool calledBack = false;
            requestQueue.RequestReceived += (s, e) =>
            {

                var req = (TestRequest)e.TwitterRequest;
                Assert.That(req.Message, Is.EqualTo(message));
                calledBack = true;
                resetEvent.Set();
            };
            requestQueue.BeginReceive();
            requestQueue.Send(new TestRequest(message));
            resetEvent.WaitOne(5000);
            Assert.IsTrue(calledBack, "didn't get the message event from the queue");
        }

        [Test, Ignore]
        public void SendOnly()
        {
            const string message = "hello, world";
            requestQueue.BeginReceive();
            requestQueue.Send(new TestRequest(message));
        }
    }
}
