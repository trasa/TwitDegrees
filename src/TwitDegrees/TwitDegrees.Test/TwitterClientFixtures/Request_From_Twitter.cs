using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Config;

namespace TwitDegrees.Test.TwitterClientFixtures
{
    [TestFixture]
    [Category("network")]
    public class Request_From_Twitter 
    {
        private TwitterClient client;

        [SetUp]
        public void SetUp()
        {
            client = new TwitterClient(new TwitterComm());    
        }

        [Test]
        public void GetFriends()
        {
            string xml = client.GetFriends("trasa", 1);
            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
            {
                Assert.Fail("didn't get a valid xml response.");
            }
            else
            {
                string name = doc.Root.Name.ToString();
                Assert.That(name, Is.EqualTo("users"));
            }
        }

        [Test]
        public void GetUser()
        {
            string xml = client.GetUser("trasa");
            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
                Assert.Fail("didn't get a valid xml response.");
            else
            {
                string screenName = doc.Root.Element("screen_name").Value;
                Assert.That(screenName, Is.EqualTo("trasa"));
            }
        }
        
        [Test]
        public void Get_Rate_Limit_Status()
        {
            new RateLimitStatusProvider(new TwitterComm(new SettingsProvider())).GetRateLimitStatus();
        }
    }
}
