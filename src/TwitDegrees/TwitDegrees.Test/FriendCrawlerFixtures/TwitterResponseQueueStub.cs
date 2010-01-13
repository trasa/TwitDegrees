using System;
using System.Collections.Generic;
using TwitDegrees.Core.Messaging;

namespace TwitDegrees.Test.FriendCrawlerFixtures
{
    public class TwitterResponseQueueStub : ITwitterResponseQueue
    {
        public TwitterResponseQueueStub()
        {
            SentResponses = new List<TwitterResponse>();
        }

        public List<TwitterResponse> SentResponses { get; private set; }

        

        public void Send(TwitterResponse response)
        {
            SentResponses.Add(response);
        }


        public void Dispose()
        {

        }

        public void BeginReceive()
        {

        }



        public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;


        // ReSharper disable UnusedMember.Local
        private void InvokeResponseReceived(ResponseReceivedEventArgs e)
        {
            EventHandler<ResponseReceivedEventArgs> received = ResponseReceived;
            if (received != null) received(this, e);
        }
        // ReSharper restore UnusedMember.Local
    }
}
