using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace TwitDegrees.Core.Api
{
    [Serializable]
    public class TwitterException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TwitterException()
        {
        }

        public TwitterException(string message) : base(message)
        {
        }

        public TwitterException(string message, Exception inner) : base(message, inner)
        {
        }

        public TwitterException(WebException webEx) : base(webEx.Message, webEx)
        {
            if (webEx.Response is HttpWebResponse)
            {
                StatusCode = (webEx.Response as HttpWebResponse).StatusCode;
            }
        }

        protected TwitterException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
