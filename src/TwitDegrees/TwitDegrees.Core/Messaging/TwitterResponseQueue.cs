using System;
using System.Messaging;

namespace TwitDegrees.Core.Messaging
{
    public interface ITwitterResponseQueue : IDisposable, ITwitterQueue
    {
        void Send(TwitterResponse response);
        event EventHandler<ResponseReceivedEventArgs> ResponseReceived;
    }

    public class TwitterResponseQueue : MsQueue, ITwitterResponseQueue
    {
        private readonly object messageLock = new object();

        public TwitterResponseQueue(string queueName) : base(queueName)
        {
            MessageQueue.ReceiveCompleted += MessageReceived;
        }

        public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;
        
        public void Send(TwitterResponse response)
        {
            SendMessage(response);
        }

        protected virtual void MessageReceived(object sender, ReceiveCompletedEventArgs e)
        {
            lock(messageLock)
            {
                OnResponseReceived(new ResponseReceivedEventArgs((TwitterResponse)e.Message.Body));
            }
        }

        protected virtual void OnResponseReceived(ResponseReceivedEventArgs e)
        {
            EventHandler<ResponseReceivedEventArgs> handle = ResponseReceived;
            if (handle != null)
                handle(this, e);
        }
    }

    public class ResponseReceivedEventArgs : EventArgs
    {
        public ResponseReceivedEventArgs(TwitterResponse response)
        {
            TwitterResponse = response;
        }

        public TwitterResponse TwitterResponse { get; private set; }
    }
}
