using System;
using System.IO;
using System.Net;
using TwitDegrees.Core.Config;
using TwitDegrees.Core.Extensions;

namespace TwitDegrees.Core.Api
{
    public interface ITwitterComm
    {
        string ExecuteGet(string url);
    }


    public class TwitterComm : ITwitterComm
    {
        private readonly string twitterUri;
        private readonly string userName;
        private readonly string password;

        public TwitterComm() : this(new SettingsProvider())
        {
        }

        public TwitterComm(ISettingsProvider settingsProvider)
        {
            twitterUri = settingsProvider.Twitter.BaseUrl;
            userName = settingsProvider.Twitter.TwitterUser;
            password = settingsProvider.Twitter.TwitterPassword;
        }

        public string ExecuteGet(string url)
        {
            url = new Uri(new Uri(twitterUri), url).ToString();
            using (var client = new WebClient())
            {
                // have to use header or the credentials aren't encoded correctly
                client.Headers["Authorization"] = new NetworkCredential(userName, password).ToAuthorizationHeader();
                try
                {
                    using (var stream = client.OpenRead(url))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            string result = streamReader.ReadToEnd();
                            if (string.IsNullOrEmpty(result))
                            {
                                throw new TwitterException("url returned null or empty.");
                            }
                            return result;
                        }
                    }
                }
                catch (WebException ex)
                {
                    // 404 
                    if (ex.Response is HttpWebResponse && (ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    // wrap WebException into something we can actually work + test with...
                    // twitter comm fails often.. tests are important.
                    throw new TwitterException(ex);
                }
            }
        }
    }
}
