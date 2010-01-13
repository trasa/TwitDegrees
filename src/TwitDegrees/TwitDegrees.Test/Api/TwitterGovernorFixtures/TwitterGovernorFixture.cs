using System;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Config;

namespace TwitDegrees.Test.Api.TwitterGovernorFixtures
{
    [TestFixture]
    public class When_Call_Can_Proceed 
    {
        [Test]
        public void CanProceed()
        {
            var provider = new Mock<IRateLimitStatusProvider>();
            provider.Setup(c => c.GetRateLimitStatus())
                .Returns(new RateLimitStatus
                {
                    HourlyLimit = 100,
                    RemainingHits = 100,
                    ResetTime = DateTime.Now.AddHours(5)
                })
                .AtMostOnce();
            var gov = new TwitterGovernor(provider.Object, new SettingsProvider());
            Assert.IsTrue(gov.CanProceed);
            provider.Verify(c => c.GetRateLimitStatus(), Times.Once());
        }

        [Test]
        public void CanProceed_Have_To_Refresh_Status()
        {
            var client = new ExpiringRateLimitStatusProvider();
            var gov = new TwitterGovernor(client, new SettingsProvider());
            Assert.IsTrue(gov.CanProceed);
            Assert.That(client.CallCount, Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class When_Call_Can_Not_Proceed
    {
        [Test]
        public void Can_Not_Proceed()
        {
            var provider = new Mock<IRateLimitStatusProvider>();
            provider.Setup(c => c.GetRateLimitStatus())
                .Returns(new RateLimitStatus
                {
                    HourlyLimit = 100,
                    RemainingHits = 0,
                    ResetTime = DateTime.Now.AddHours(5)
                })
                .AtMostOnce();
            var gov = new TwitterGovernor(provider.Object,
                                          new StubSettingsProvider(new TwitterSection {RateLimitThreshold = 0}));
            
            Assert.IsFalse(gov.CanProceed);
            provider.Verify(c => c.GetRateLimitStatus(), Times.Once());
        }
    }



    internal class ExpiringRateLimitStatusProvider : IRateLimitStatusProvider
    {
        public int CallCount { get; private set; }
        public RateLimitStatus GetRateLimitStatus()
        {
            CallCount++;
            switch (CallCount)
            {
                case 1:

                    return new RateLimitStatus
                    {
                        HourlyLimit = 100,
                        RemainingHits = 0,
                        ResetTime = DateTime.Now.AddHours(-1).ToUniversalTime()
                    };
                case 2:
                    return new RateLimitStatus
                    {
                        HourlyLimit = 100,
                        RemainingHits = 100,
                        ResetTime = DateTime.Now.AddHours(1).ToUniversalTime()
                    };
                default:
                    throw new Exception("Too many calls to ExpiringTwitterClient.GetRateLimitStatus (" + CallCount + ")");
            }
        }
    }
}
