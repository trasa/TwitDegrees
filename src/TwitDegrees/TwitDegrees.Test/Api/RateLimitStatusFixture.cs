using System;
using System.Reflection;
using log4net;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.Api
{
    [TestFixture]
    public class RateLimitStatusFixture
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void Is_Not_Valid()
        {
            var status = new RateLimitStatus {ResetTime = DateTime.Now.ToUniversalTime().AddHours(-1)};
            log.DebugFormat("Reset Time is " + status.ResetTime);
            log.DebugFormat("Current Time is " + DateTime.Now.ToUniversalTime());
            Assert.That(status.IsValid, Is.False);
        }

        [Test]
        public void Is_Valid()
        {
            var status = new RateLimitStatus { ResetTime = DateTime.Now.ToUniversalTime().AddHours(1) };
            log.DebugFormat("Reset Time is " + status.ResetTime);
            log.DebugFormat("Current Time is " + DateTime.Now.ToUniversalTime());
            Assert.That(status.IsValid, Is.True);
        }

        [Test]
        public void Is_Time_Valid_Past_Midnight()
        {
            var resetTime = DateTimeOffset.Parse("2009-06-03T07:23:12+00:00");
            var now = new DateTime(2009, 06, 03, 00, 28, 0);
            Assert.IsTrue(now < resetTime.LocalDateTime);

            

        }
    }
}
