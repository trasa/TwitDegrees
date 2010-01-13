using System;


namespace TwitDegrees.Core.Api
{
    public class RateLimitStatus
    {
        public int HourlyLimit { get; set; }
        public DateTimeOffset ResetTime { get; set; }
        public int RemainingHits { get; set; }

        public bool IsValid
        {
            get
            {
                return DateTime.Now < ResetTime.LocalDateTime;
            }
        }
    }
}
