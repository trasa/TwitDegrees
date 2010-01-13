using System;
using TwitDegrees.Core.Messaging;

namespace TwitDegrees.Test.Messaging
{
    public class MockResponseQueue : MockQueue<TwitterResponse>, ITwitterResponseQueue
    {
        public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

        public void InvokeResponseReceived(ResponseReceivedEventArgs e)
        {
            EventHandler<ResponseReceivedEventArgs> received = ResponseReceived;
            if (received != null) received(this, e);
        }
    }
}
