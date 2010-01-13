using System;
using System.Reflection;
using System.Threading;
using log4net;
using TwitDegrees.Core.Config;

namespace TwitDegrees.Core.Api
{
    public interface ITwitterGovernor
    {
        bool CanProceed { get; }
        void WaitForClearance();
    }

    #region ITwitterGovernors for Testing

    public class PermissiveTwitterGovernor : ITwitterGovernor
    {
        public bool CanProceed
        {
            get { return true; }
        }

        public void WaitForClearance()
        {
            // granted!
        }
    }

    public class SwitchedTwitterGovernor : ITwitterGovernor
    {
        private int blockThisManyRequests;
        private readonly object proceedLock = new object();

        public SwitchedTwitterGovernor(int blockThisManyRequests)
        {
            this.blockThisManyRequests = blockThisManyRequests;
        }

        public bool CanProceed
        {
            get
            {
                lock (proceedLock)
                {
                    bool result = blockThisManyRequests <= 0;
                    Interlocked.Decrement(ref blockThisManyRequests);
                    return result;
                }
            }
        }

        public void WaitForClearance()
        {
            while (!CanProceed)
            {
                Thread.Sleep(1000);
            }

        }
    }

    #endregion

    public class TwitterGovernor : ITwitterGovernor
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRateLimitStatusProvider rateLimitProvider;
        private RateLimitStatus status;
        private readonly object governorLock = new object();

        private readonly int threshold; 

        public TwitterGovernor(IRateLimitStatusProvider rateLimitProvider, ISettingsProvider settingsProvider)
        {
            this.rateLimitProvider = rateLimitProvider;
            status = rateLimitProvider.GetRateLimitStatus();

            threshold = settingsProvider.Twitter.RateLimitThreshold;
        }

        


        private void RefreshStatus()
        {
            if (status.IsValid)
                return;
            log.Info("Refreshing Rate Limit Status");
            status = rateLimitProvider.GetRateLimitStatus();
        }


        public void WaitForClearance()
        {
            while (!CanProceed)
            {
                DateTime localResetTime = status.ResetTime.LocalDateTime;
                log.Info("Waiting For clearance to proceed with Twitter API call, sleeping until " + localResetTime);
                double sleepInMs = (localResetTime - DateTime.Now).TotalMilliseconds;
                if (sleepInMs <= 0)
                {
                    sleepInMs = 1000;
                } 
                else if (sleepInMs > int.MaxValue)
                {
                    sleepInMs = int.MaxValue;
                }
                log.Info("Sleeping for " + sleepInMs + " ms");
                Thread.Sleep(Convert.ToInt32(sleepInMs));
            }
        }

        public bool CanProceed
        {
            get
            {
                lock (governorLock)
                {
                    RefreshStatus();
                    log.Debug("Checking Proceed Status, Remaining Hits Available: " + status.RemainingHits);
                    if (status.RemainingHits <= threshold)
                        return false;
                    status.RemainingHits--; // don't decrement unnecessarily
                    return status.RemainingHits > 0;
                }
            }
        }
    }
}
