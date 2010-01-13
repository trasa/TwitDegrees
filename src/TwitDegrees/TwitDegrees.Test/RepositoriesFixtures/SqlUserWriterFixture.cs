using System.Collections.Generic;
using NUnit.Framework;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Repositories;

namespace TwitDegrees.Test.RepositoriesFixtures
{
    [TestFixture]
    public class SqlUserWriterFixture
    {
        [Test]
        public void Write_Friends()
        {
            var writer = new SqlUserWriter();
            writer.WriteUserInfo(new UserInfo
                                     {
                                         Name = "test",
                                         FollowerCount = 6,
                                         FriendCount = 2,
                                         Location = "Here"
                                     });
            writer.WriteFriends("test", new List<UserInfo>
                                            {
                                                new UserInfo {Name = "friend1"},
                                                new UserInfo {Name = "friend2", Location ="xyz"},
                                            });
        }

        [Test]
        public void Write_User()
        {
            var writer = new SqlUserWriter();
            writer.WriteUserInfo(new UserInfo
                                     {
                                         Name = "test",
                                         FollowerCount = 6,
                                         FriendCount = 2,
                                         Location = "Here"
                                     });
        }
    }
}
