using System.Configuration;

namespace TwitDegrees.Core.Config
{
    public class TwitterSection : ConfigurationSection
    {
        [ConfigurationProperty("RateLimitThreshold", DefaultValue=10, IsRequired=false)]
        public int RateLimitThreshold
        {
            get { return (int)this["RateLimitThreshold"]; }
            set { this["RateLimitThreshold"] = value; }
        }

        [ConfigurationProperty("BaseUrl", DefaultValue="http://twitter.com", IsRequired=false)]
        public System.String BaseUrl
        {
            get
            {
                var url = (System.String)this["BaseUrl"];
                if (url.EndsWith("/"))
                    url = url.Substring(0, url.Length - 1);
                return url;
            }
            set { this["BaseUrl"] = value; }
        }

        [ConfigurationProperty("TwitterUser", DefaultValue="twitdegrees", IsRequired=false)]
        public System.String TwitterUser
        {
            get { return (System.String)this["TwitterUser"]; }
            set { this["TwitterUser"] = value; }
        }

        [ConfigurationProperty("TwitterPassword", DefaultValue="blaackf1n", IsRequired=false)]
        public System.String TwitterPassword
        {
            get { return (System.String)this["TwitterPassword"]; }
            set { this["TwitterPassword"] = value; }
        }

        [ConfigurationProperty("RequestQueueName", DefaultValue = @"FormatName:DIRECT=OS:sqldev\private$\TwitterRequest", IsRequired = false)]
        public System.String RequestQueueName
        {
            get { return (System.String)this["RequestQueueName"]; }
            set { this["RequestQueueName"] = value; }
        }

        [ConfigurationProperty("ResponseQueueName", DefaultValue = @"FormatName:DIRECT=OS:sqldev\private$\TwitterResponse", IsRequired = false)]
        public System.String ResponseQueueName
        {
            get { return (System.String)this["ResponseQueueName"]; }
            set { this["ResponseQueueName"] = value; }
        }

    }
}
