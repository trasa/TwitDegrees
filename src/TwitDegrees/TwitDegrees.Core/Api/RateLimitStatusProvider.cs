using System;
using System.Reflection;
using System.Xml.Linq;
using log4net;

namespace TwitDegrees.Core.Api
{
    public interface IRateLimitStatusProvider
    {
        RateLimitStatus GetRateLimitStatus();
    }

    public class RateLimitStatusProvider : IRateLimitStatusProvider
    {
        private readonly ITwitterComm twitterComm;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RateLimitStatusProvider(ITwitterComm twitterComm)
        {
            this.twitterComm = twitterComm;
        }

        public RateLimitStatus GetRateLimitStatus()
        {
            // goes to twitter comm directly - no limiting
            string xml = twitterComm.ExecuteGet("/account/rate_limit_status.xml");

            var hash = XDocument.Parse(xml).Root;
            var result = new RateLimitStatus
                             {
                                 // ReSharper disable PossibleNullReferenceException
                                 HourlyLimit = int.Parse(hash.Element("hourly-limit").Value),
                                 ResetTime = DateTimeOffset.Parse(hash.Element("reset-time").Value),
                                 RemainingHits = int.Parse(hash.Element("remaining-hits").Value),
                                 // ReSharper restore PossibleNullReferenceException
                             };
            if (!result.IsValid)
            {
                log.WarnFormat("Retrieved RateLimitStatus from Twitter, but it's already not valid! Now: {0} Reset (LocalDateTime): {1} Reset(ToLocalTime): {2}",
                               DateTime.Now,
                               result.ResetTime.LocalDateTime,
                               result.ResetTime.ToLocalTime());
				// problem seems to go away after a little bit (maybe 1 minute? possible bug somewhere..) so forget it, 
				// we'll try again in a few minutes.
				result.ResetTime = DateTime.Now.AddMinutes(5);
            }
            return result;
        }
    }



    public class PermissiveRateLimitStatusProvider : IRateLimitStatusProvider
    {
        public RateLimitStatus GetRateLimitStatus()
        {
            return new RateLimitStatus
                       {
                           HourlyLimit = 20000,
                           RemainingHits = 20000,
                           ResetTime = DateTime.Now.AddMonths(1)
                       };
        }
    }
}
