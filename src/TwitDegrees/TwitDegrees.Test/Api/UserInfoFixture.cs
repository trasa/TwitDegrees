using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.Api
{
    [TestFixture]
    public class UserInfoFixture
    {
        [Test]
        public void Location_Is_Null()
        {
            var user = new UserInfo {Location = null};
            Assert.That(user.Location, Is.Null);
        }

        [Test]
        public void Location_Is_Normal()
        {
            var user = new UserInfo { Location = "normal"};
            Assert.That(user.Location, Is.EqualTo("normal"));
        }

        [Test]
        public void Location_Is_Truncated()
        {
            var user = new UserInfo {Location = new string('z', 255)};
            Assert.That(user.Location.Length, Is.EqualTo(254));

            user.Location = new string('z', 1000);
            Assert.That(user.Location.Length, Is.EqualTo(254));

            user.Location = new string('z', 254);
            Assert.That(user.Location.Length, Is.EqualTo(254));
        }

        [Test]
        public void Location_Is_Empty()
        {
            var user = new UserInfo {Location = string.Empty};
            Assert.That(user.Location, Is.EqualTo(string.Empty));
        }




        [Test]
        public void Name_Is_Null()
        {
            var user = new UserInfo { Name = null };
            Assert.That(user.Name, Is.Null);
        }

        [Test]
        public void Name_Is_Normal()
        {
            var user = new UserInfo { Name = "normal" };
            Assert.That(user.Name, Is.EqualTo("normal"));
        }

        [Test]
        public void Name_Is_Truncated()
        {
            var user = new UserInfo { Name = new string('z', 255) };
            Assert.That(user.Name.Length, Is.EqualTo(254));

            user.Name = new string('z', 1000);
            Assert.That(user.Name.Length, Is.EqualTo(254));

            user.Name = new string('z', 254);
            Assert.That(user.Name.Length, Is.EqualTo(254));
        }

        [Test]
        public void Name_Is_Empty()
        {
            var user = new UserInfo { Name = string.Empty };
            Assert.That(user.Name, Is.EqualTo(string.Empty));
        }
    }
}
