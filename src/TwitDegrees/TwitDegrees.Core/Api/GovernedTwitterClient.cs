using System;

namespace TwitDegrees.Core.Api
{
    public class GovernedTwitterClient : TwitterClient
    {
        private delegate string AsyncExecuteGet(string url);

        private readonly ITwitterGovernor governor;

        public GovernedTwitterClient(ITwitterComm twitterComm, ITwitterGovernor governor) : base(twitterComm)
        {
            this.governor = governor;
        }

        protected override string ExecuteGet(string url)
        {
            var asyncGet = new AsyncExecuteGet(GetResults);
            IAsyncResult result = asyncGet.BeginInvoke(url, null, null);
            result.AsyncWaitHandle.WaitOne();
            return asyncGet.EndInvoke(result);
        }

        private string GetResults(string url)
        {
            governor.WaitForClearance();
            return base.ExecuteGet(url);
        }
    }
}
