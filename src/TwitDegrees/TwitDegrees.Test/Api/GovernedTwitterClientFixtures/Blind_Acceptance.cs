using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.Api.GovernedTwitterClientFixtures
{
    [TestFixture]
    public class Blind_Acceptance : When_Requesting_User
    {
        protected override ITwitterGovernor Governor
        {
            get { return new PermissiveTwitterGovernor(); }
        }

        [Test]
        public void Got_Result()
        {
            Assert.That(Result, Is.EqualTo(Original));
        }
    }
}
