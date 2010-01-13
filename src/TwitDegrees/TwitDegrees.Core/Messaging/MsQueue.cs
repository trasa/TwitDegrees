using System;
using System.Messaging;

namespace TwitDegrees.Core.Messaging
{
    public class MsQueue : IDisposable
    {
        protected MessageQueue MessageQueue { get; private set; }

        public MsQueue(string queueName)
        {
            MessageQueue = new MessageQueue(queueName)
                        {
                            MessageReadPropertyFilter = {AppSpecific = true},
                            Formatter = new BinaryMessageFormatter()
                        };
        }

        public void BeginReceive()
        {
            if (alreadyDisposed) throw new ObjectDisposedException("MsQueue");
            MessageQueue.BeginReceive();
        }

        protected void SendMessage(object message)
        {
            if (alreadyDisposed) throw new ObjectDisposedException("MsQueue");

            // Since we're using a transactional queue, make a transaction.
            using (var transaction = new MessageQueueTransaction())
            {
                transaction.Begin();
                var myMessage = new Message(message, new BinaryMessageFormatter())
                {
                    Label = message.ToString(),
                    AppSpecific = 0, 
                    Recoverable = true,
                    AttachSenderId = false,
                };
                // Send the message. transactional stuff is important, wont work without it..
                MessageQueue.Send(myMessage, transaction);
                transaction.Commit();
            }
        }



        #region IDisposable, members and implementation

        ///<summary>
        /// Has this object already been disposed?  (don't need to dispose twice)
        ///</summary>
        private bool alreadyDisposed;

        /// <summary>
        /// Safely dispose of this object.
        /// </summary>
        public void Dispose()
        {
            // call the dispose mechanism, informing it that we're coming
            // from IDisposable instead of the Finalizer
            Dispose(true);

            // since we're disposed, don't need to involve the Finalizer Thread.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:TwitDegrees.Core.Messaging.MsQueue"/> is reclaimed by garbage collection.
        /// </summary>
        ~MsQueue()
        {
            // defensive technique:  if the user forgets to call this.Dispose(),
            // we must clean up when (if!) the GC cleans this object.
            // call Dispose mechanism, saying that we're coming from the finalizer
            Dispose(false);
        }

        /// <summary>
        /// Safely disposes of this object.
        /// </summary>
        /// <param name="isDisposing">
        /// If set to <c>true</c>, object is being disposed by Dispose pattern, else object is being disposed
        /// by finalizer thread.
        /// </param>
        protected virtual void Dispose(bool isDisposing)
        {
            // don't dispose more than once
            if (alreadyDisposed)
                return;

            if (isDisposing)
            {
                // called from IDisposable - safe to free Managed Resources here.
                if (MessageQueue != null)
                {
                    MessageQueue.Dispose();
                    MessageQueue = null;
                }
            }
            // TODO: Free UNMANAGED resources here!

            alreadyDisposed = true;
        }

        #endregion
    }
}
