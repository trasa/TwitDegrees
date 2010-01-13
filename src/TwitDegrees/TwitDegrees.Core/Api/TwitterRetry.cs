using System;
using System.Reflection;
using System.Threading;
using Blackfin.Core.Exceptions;
using log4net;

namespace TwitDegrees.Core.Api
{
    public class TwitterRetry
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TwitterRetry(int maxTries)
        {
            MaxTries = maxTries;
        }

        public int MaxTries { get; set; }

        public TResult Try<TResult>(Func<TResult> action)
        {
            int tries = 0;
            while (tries < MaxTries)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    tries++;
                    string logMsg = "Retry Attempt " + tries;
                    if (ex is TwitterException)
                    {
                        logMsg += ", Twitter returned code " + (ex as TwitterException).StatusCode;
                    }
                    log.Warn(logMsg, ex);

                    if (tries >= MaxTries)
                        throw; // give up

                    int timeout = 10 * 1000 * tries;
                    log.Warn("Sleeping for " + timeout + " ms");
                    Thread.Sleep(timeout);
                }
            }
            // this point should never happen..
            // either we get a result, we get a non-TwitterException (and get thrown out), or we get maxtries and throw TwitterException.
            throw new InvalidStateException("Error condition in Try() escaped from custody.");
        }
    }
}
