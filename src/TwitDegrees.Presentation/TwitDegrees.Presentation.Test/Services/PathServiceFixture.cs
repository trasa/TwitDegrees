using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Presentation.Core.Models;
using TwitDegrees.Presentation.Core.Repositories;
using TwitDegrees.Presentation.Core.Services;

namespace TwitDegrees.Presentation.Test.Services
{
    [TestFixture]
    public class PathServiceFixture
    {
        [Test]
        public void To_Self()
        {
            var userPathRepo = new Mock<IUserPathRepository>();
            var pathService = new PathService(userPathRepo.Object);

            IEnumerable<string> result = pathService.FindPath("start", "start");

            Assert.That(result, Has.Count(1));
            Assert.That(result.First(), Is.EqualTo("start"));
        }

        [Test]
        public void Simple_Path()
        {
            var userPathRepo = new Mock<IUserPathRepository>();
            var pathService = new PathService(userPathRepo.Object);
            var startFriends = new FriendDictionary(new List<TwitterUser>
                                                        {
                                                            new TwitterUser {Name = "dest", FriendCount = 100}
                                                        });
            userPathRepo.Setup(repo => repo.GetFriendsOf("start")).Returns(startFriends);

            IEnumerable<string> result = pathService.FindPath("start", "dest");

            Assert.That(result, Has.Count(2));
            Assert.That(result.First(), Is.EqualTo("start"));
            Assert.That(result.Last(), Is.EqualTo("dest"));

        }

        [Test]
        public void Has_Loop()
        {
            var userPathRepo = new Mock<IUserPathRepository>();
            var pathService = new PathService(userPathRepo.Object);
            var afriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "b", FriendCount = 100},
                                                        new TwitterUser {Name = "d", FriendCount = 105},
                                                    });
            var bfriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "a", FriendCount = 100},
                                                        new TwitterUser {Name = "c", FriendCount = 100},
                                                    });
            var dfriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "a", FriendCount = 100},
                                                    });
            userPathRepo.Setup(repo => repo.GetFriendsOf("a")).Returns(afriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("b")).Returns(bfriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("d")).Returns(dfriends);

            List<string> result = pathService.FindPath("a", "c").ToList();

            Assert.That(result, Has.Count(3));
            Assert.That(result.First(), Is.EqualTo("a"));
            Assert.That(result[1], Is.EqualTo("b"));
            Assert.That(result.Last(), Is.EqualTo("c"));
        }

        [Test]
        public void No_Path_Exists()
        {
            var userPathRepo = new Mock<IUserPathRepository>();
            var pathService = new PathService(userPathRepo.Object);
            var afriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "b", FriendCount = 100},
                                                        new TwitterUser {Name = "d", FriendCount = 105},
                                                    });
            var bfriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "a", FriendCount = 100},
                                                        new TwitterUser {Name = "c", FriendCount = 100},
                                                    });
            var cfriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "a", FriendCount = 100},
                                                    });
            var dfriends = new FriendDictionary(new List<TwitterUser>
                                                    {
                                                        new TwitterUser {Name = "a", FriendCount = 100},
                                                    });
            var efriends = new FriendDictionary(new List<TwitterUser>());

            userPathRepo.Setup(repo => repo.GetFriendsOf("a")).Returns(afriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("b")).Returns(bfriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("c")).Returns(cfriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("d")).Returns(dfriends);
            userPathRepo.Setup(repo => repo.GetFriendsOf("e")).Returns(efriends);

            IEnumerable<string> result = pathService.FindPath("a", "e");

            Assert.That(result, Is.Null);
        }
    }
}
