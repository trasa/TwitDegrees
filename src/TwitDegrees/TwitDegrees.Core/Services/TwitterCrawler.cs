using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Messaging;

namespace TwitDegrees.Core.Services
{
    public class TwitterCrawler
    {
        private readonly ITwitterRequestQueue requestQueue;
        private readonly ITwitterResponseQueue responseQueue;
        private readonly IStatusService statusService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object requestLock = new object();


        public TwitterCrawler(ITwitterRequestQueue requestQueue, ITwitterResponseQueue responseQueue, IStatusService statusService)
        {
            if (requestQueue == null)
                throw new ArgumentNullException("requestQueue");
            if (responseQueue == null)
                throw new ArgumentNullException("responseQueue");

            this.requestQueue = requestQueue;
            this.responseQueue = responseQueue;
            this.statusService = statusService;

            this.requestQueue.RequestReceived += (s, e) => ProcessRequest(e.TwitterRequest);
            this.requestQueue.BeginReceive();
            
        }

        public void ProcessRequest(TwitterRequest request)
        {
            lock (requestLock)
            {
                try
                {
                    log.Info("Received Request: " + request);
                    if (request is GetFriendsRequest)
                    {
                        ProcessGetFriendsRequest(request as GetFriendsRequest);
                    }
                    else if (request is GetFollowersRequest)
                    {
                        ProcessGetFollowersRequest(request as GetFollowersRequest);
                    }
                    else if (request is UserInfoRequest)
                    {
                        ProcessUserInfoRequest(request as UserInfoRequest);
                    }
                    else if (request is TestRequest)
                    {
                        // nothing to do
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("request", "Unknown or unhandled request type: " + request.GetType().Name);
                    }
                } 
                catch(Exception ex)
                {
                    responseQueue.Send(new ErrorResponse(request, "Exception: " + ex.GetType().Name, ex));
                }

                requestQueue.BeginReceive(); // next..
            }
        }


        private void ProcessGetFriendsRequest(GetFriendsRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IEnumerable<UserInfo> friends = statusService.GetFriendsOf(request.UserName);
            if (friends == null)
            {
                log.Warn("Can't get friends of " + request.UserName + " - returned null.");
                responseQueue.Send(new ErrorResponse(request, "GetFriendsOf returned null."));
            } 
            else
            {
                responseQueue.Send(new GetFriendsResponse(request.UserName, friends));
                ReportStopwatchResult(stopwatch, "GetFriends", friends.Count());
            }
        }


        private void ProcessGetFollowersRequest(GetFollowersRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IEnumerable<UserInfo> followers = statusService.GetFollowersOf(request.UserName);
            if (followers == null)
            {
                log.Warn("Can't get followers of " + request.UserName + " - returned null.");
                responseQueue.Send(new ErrorResponse(request, "GetFollowersOf returned null."));
            }
            else
            {
                responseQueue.Send(new GetFollowersResponse(request.UserName, followers));
                ReportStopwatchResult(stopwatch, "GetFollowers", followers.Count());
            }
        }

        private static void ReportStopwatchResult(Stopwatch stopwatch, string processName, int recordCount)
        {
            stopwatch.LogResult(ms => log.DebugFormat("{0} (ms/friend),ms,count: {1:0.00000},{2:0.00},{3}",
                                                      processName,
                                                      ms / (recordCount == 0 ? 1 : recordCount),
                                                      ms,
                                                      recordCount
                                          ));
        }

        public void ProcessUserInfoRequest(UserInfoRequest request)
        {
            UserInfo user = statusService.GetUser(request.UserName);
            if (user == null)
            {
                string msg = "Can't find UserInfo for " + request.UserName + ", they're protected or don't exist or something.  Giving up.";
                log.Warn(msg);
                responseQueue.Send(new ErrorResponse(request, msg));
            }
            else
            {
                responseQueue.Send(new UserInfoResponse(user));
            }
        }
    }
}
