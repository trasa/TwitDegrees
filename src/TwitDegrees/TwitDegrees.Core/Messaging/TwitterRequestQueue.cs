using System;
using System.Messaging;

namespace TwitDegrees.Core.Messaging
{
    public interface ITwitterRequestQueue : IDisposable, ITwitterQueue
    {
        void Send(TwitterRequest request);
        event EventHandler<RequestReceivedEventArgs> RequestReceived;
    }

    public class TwitterRequestQueue  : MsQueue, ITwitterRequestQueue
    {
        public event EventHandler<RequestReceivedEventArgs> RequestReceived;
        private readonly object receiveLock = new object();
        
        public TwitterRequestQueue(string queueName) : base(queueName)
        {
            MessageQueue.ReceiveCompleted += MessageReceived;
        }

        public void Send(TwitterRequest request)
        {
            SendMessage(request);
        }

        protected virtual void MessageReceived(object sender, ReceiveCompletedEventArgs e)
        {
            lock(receiveLock)
            {
                OnRequestReceived(new RequestReceivedEventArgs((TwitterRequest)e.Message.Body));
            }
        }

        protected virtual void OnRequestReceived(RequestReceivedEventArgs e)
        {
            EventHandler<RequestReceivedEventArgs> handle = RequestReceived;
            if (handle != null)
                handle(this, e);
        }
    }

    public class RequestReceivedEventArgs : EventArgs
    {
        public RequestReceivedEventArgs(TwitterRequest request)
        {
            TwitterRequest = request;
        }

        public TwitterRequest TwitterRequest { get; private set; }
    }
}
