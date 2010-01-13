using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Presentation.Core.Models;

namespace TwitDegrees.Presentation.Test.Models
{
    [TestFixture]
    public class UserGraphFixture
    {
        [Test]
        public void Simple_Graph()
        {
            var graph = new UserGraph();
            graph.Add("test", "test");

            Assert.That(graph.Users, Has.Count(1));
            Assert.That(graph.Edges.Count(), Is.EqualTo(1));
        }

        [Test]
        public void One_Edge()
        {
            var root = "test";
            var friend = "friend";
            var graph = new UserGraph();
            graph.Add(root, friend);
            Assert.That(graph.Users, Has.Count(2));
            Assert.That(graph.Users.Contains(root));
            Assert.That(graph.Users.Contains(friend));
            Assert.That(graph.Edges.Count(), Is.EqualTo(1));
            Assert.That(graph.Edges.First().Source, Is.EqualTo(root));
            Assert.That(graph.Edges.First().Target, Is.EqualTo(friend));
        }

        [Test]
        public void Two_Edges()
        {
            var root = "test";
            var friend = "friend";
            var otherFriend = "otherfriend";
            var graph = new UserGraph();
            graph.Add(root, friend);
            graph.Add(root, otherFriend);
            Assert.That(graph.Users, Has.Count(3));
            Assert.That(graph.Edges.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Two_Levels()
        {
            var root = "test";
            var friend = "friend";
            var otherFriend = "otherfriend";
            var graph = new UserGraph();
            graph.Add(root, friend);
            graph.Add(friend, otherFriend);
            Assert.That(graph.Users, Has.Count(3));
            Assert.That(graph.Edges.Count(), Is.EqualTo(2));
        }
    }
}
