using System;
using TwitDegrees.Core.Messaging;

namespace TwitDegrees.Test.Messaging
{
    public class MockRequestQueue : MockQueue<TwitterRequest>, ITwitterRequestQueue
    {
        public event EventHandler<RequestReceivedEventArgs> RequestReceived;

        public void InvokeRequestReceived(RequestReceivedEventArgs e)
        {
            EventHandler<RequestReceivedEventArgs> received = RequestReceived;
            if (received != null) received(this, e);
        }
    }
}
