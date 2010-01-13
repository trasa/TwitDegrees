using System;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class ErrorResponse : TwitterResponse
    {
        public ErrorResponse(TwitterRequest request, string message) 
            :this(request, message, null)
        {
        }
        
        public ErrorResponse(TwitterRequest request, string message, Exception exception)
        {
            Request = request;
            Message = message;
            Exception = exception;
        }

        public TwitterRequest Request { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }

        public override string ToString()
        {
            return "ErrorResponse: " + Message;
        }
    }
}
