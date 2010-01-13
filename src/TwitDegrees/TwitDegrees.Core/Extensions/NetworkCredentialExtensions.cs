using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TwitDegrees.Core.Extensions
{
    public static class NetworkCredentialExtensions
    {
        public static string ToAuthorizationHeader(this NetworkCredential credentials)
        {
            var token = "{0}:{1}".FormatWith(credentials.UserName, credentials.Password).GetBytes().ToBase64String();
            return "Basic {0}".FormatWith(token);
        }
    }
}
