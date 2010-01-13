using System.Collections.Generic;

namespace TwitDegrees.Test.Messaging
{
    public class MockQueue<TMessage>
    {
        public MockQueue()
        {
            QueuedMessages = new List<TMessage>();
        }

        public List<TMessage> QueuedMessages { get; private set; }


        public void Dispose()
        {

        }

        public void BeginReceive()
        {
        }

        public void Send(TMessage request)
        {
            QueuedMessages.Add(request);
        }
    }
}
