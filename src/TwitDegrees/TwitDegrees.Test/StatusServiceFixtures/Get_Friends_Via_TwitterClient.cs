using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.StatusServiceFixtures
{
    [TestFixture]
    [Category("network")]
    public class Get_Friends_Via_TwitterClient
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public void SetUp()
        {
            StatusService = new StatusService(new TwitterClient(new TwitterComm()), new PermissiveRateLimitStatusProvider());
        }

        protected StatusService StatusService { get; set; }


        [Test]
        public void GetFriends()
        {
            IEnumerable<UserInfo> friends = StatusService.GetFriendsOf("trasa");
            Assert.That(friends.Count(), Is.GreaterThan(100));
            log.Debug("Found " + friends.Count()  + " friends of trasa");
        }

        [Test]
        public void GetFriendsFor_Some_User()
        {
            IEnumerable<UserInfo> friends = StatusService.GetFriendsOf("jadebarclay");
            Assert.That(friends.Count(), Is.GreaterThan(1000));
            log.Debug("Found " + friends.Count() + " friends of jadebarclay");
        }
    }
}
