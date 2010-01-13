using System;

namespace TwitDegrees.Core.Messaging
{
    [Serializable]
    public class TestRequest : TwitterRequest
    {
        public TestRequest(string msg)
        {
            Message = msg;
        }

        public string Message { get; private set; }

        public override string ToString()
        {
            return "TestRequest: " + Message;
        }
    }
}