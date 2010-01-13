using System;
using System.Reflection;
using System.Threading;
using log4net;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Repositories;

namespace TwitDegrees.Core.Services
{
    public class CrawlerController
    {
        private readonly IUserWriter userWriter;
        private readonly ITwitterRequestQueue requestQueue;
        private readonly ITwitterResponseQueue responseQueue;
        private readonly IRecentFriendRegistry recentFriendRegistry;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object responseLock = new object();

        public CrawlerController(IUserWriter userWriter,
            ITwitterRequestQueue requestQueue,
            ITwitterResponseQueue responseQueue,
            IRecentFriendRegistry recentFriendRegistry)
        {
            this.userWriter = userWriter;
            this.requestQueue = requestQueue;
            this.responseQueue = responseQueue;
            this.recentFriendRegistry = recentFriendRegistry;

            this.responseQueue.ResponseReceived += (s, e) => ProcessResponse(e.TwitterResponse);
            this.responseQueue.BeginReceive();
        }

        /// <summary>
        /// Inform the Controller that it's quitting time.  
        /// In this case, the Application is telling the Controller that we should quit now, wrap things up.
        /// </summary>
        /// <value><c>true</c> if time to attempt shutdown; otherwise, <c>false</c>.</value>
        public bool Shutdown { get; set; }

        /// <summary>
        /// The indicator for the controlling application that the crawler should be terminated.
        /// In this case, the Controller is telling the application that it's time to go.
        /// </summary>
        /// <value>The shutdown reset event.</value>
        public ManualResetEvent ShutdownResetEvent { get; set; }

        public bool EnableFriendCrawling
        {
            get { return recentFriendRegistry.IsCrawlEnabled; }
            set { recentFriendRegistry.IsCrawlEnabled = value; }
        }

        // TODO!
        public bool EnableFollowerCrawling { get; set; }


        public void Crawl(string userName)
        {
            recentFriendRegistry.RegisterRecentFriend(userName);
            requestQueue.Send(new UserInfoRequest(userName));
            requestQueue.Send(new GetFriendsRequest(userName));
        }


        public void CrawlFriendsOf(string userName)
        {
            if (!recentFriendRegistry.ShouldCrawlThisUser(userName))
            {
                return;
            }

            log.Debug("request friends for " + userName);
            requestQueue.Send(new GetFriendsRequest(userName));
        }

        public void ProcessResponse(TwitterResponse response)
        {
            try
            {
                lock (responseLock)
                {
                    log.Info("Received Response: " + response);
                    if (response is GetFriendsResponse)
                    {
                        ProcessGetFriendsResponse(response as GetFriendsResponse);
                    }
                    else if (response is GetFollowersResponse)
                    {
                        ProcessGetFollowersResponse(response as GetFollowersResponse);
                    }
                    else if (response is UserInfoResponse)
                    {
                        ProcessUserInfoResponse(response as UserInfoResponse);
                    }
                    else if (response is ErrorResponse)
                    {
                        ProcessErrorResponse(response as ErrorResponse);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("response", "Unknown / unhandled response type: " + response.GetType().Name);
                    }

                    if (!Shutdown)
                    {
                        responseQueue.BeginReceive(); // next..
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Unhandled exception while trying to process result.  This will hang up the ResponseQueue.BeginReceive() loop, attempting to terminate program.", ex);
                Shutdown = true;
                if (ShutdownResetEvent != null)
                {
                    ShutdownResetEvent.Set();
                }
            }
        }


        private void ProcessGetFriendsResponse(GetFriendsResponse response)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            recentFriendRegistry.RegisterRecentFriend(response.UserName);
            userWriter.WriteFriends(response.UserName, response.FriendsOfUser);
            foreach (var friend in response.FriendsOfUser)
            {
                CrawlFriendsOf(friend.Name);
            }

            int friendCount = response.FriendsOfUser.Length;
            ReportStopwatchResult(stopwatch, "GetFriends", friendCount);
        }

        private static void ReportStopwatchResult(Stopwatch stopwatch, string processName, int count)
        {
            stopwatch.LogResult(ms => log.InfoFormat("{0} (ms/user),ms,usercount: {1:0.00000},{2:0.00},{3}",
                                                     processName,
                                                     ms / count == 0 ? 1 : count,
                                                     ms,
                                                     count));
        }


        private void ProcessGetFollowersResponse(GetFollowersResponse response)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // TODO: recent friends registry (for controlling the crawler requests)
            userWriter.WriteFollowers(response.UserName, response.FollowersOfUser);
            // TODO: crawling
            ReportStopwatchResult(stopwatch, "GetFollowers", response.FollowersOfUser.Length);
        }



        private void ProcessErrorResponse(ErrorResponse response)
        {
            // requeue the work, unless we've already retried this message too many times, 
            // in that case, forget it.
            log.Error("ErrorResponse received: " + response, response.Exception);
            log.Error("Retrying Error'd Request: " + response.Request);
            requestQueue.Send(response.Request);
        }


        private void ProcessUserInfoResponse(UserInfoResponse response)
        {
            userWriter.WriteUserInfo(response.UserInfo);
        }
    }
}
