using System;
using System.Linq;
using NUnit.Framework;
using TwitDegrees.Presentation.Core.Models;
using TwitDegrees.Presentation.Core.Services;

namespace TwitDegrees.Presentation.Test.Services
{
    [TestFixture]
    public class GraphServiceFixture 
    {

        public GraphService ServiceUnderTest { get; set; }

        [SetUp]
        public void SetUp()
        {
            ServiceUnderTest = new GraphService();
        }


        [Test]
        public void Build_Simplest_Graph()
        {
            var graph = new UserGraph();
            graph.Add("root", "root");
            string result = ServiceUnderTest.BuildGraphML(graph);
            Console.WriteLine(result);
        }

        [Test]
        public void Build_Simplest_Graph_With_Edges()
        {
            const string root = "test";
            const string friend = "friend";
            var graph = new UserGraph();
            graph.Add(root, friend);
            string result = ServiceUnderTest.BuildGraphML(graph);
            Console.WriteLine(result);
        }

        [Test]
        public void Hit_Database_And_Build_Graph()
        {
            UserGraph graph = ServiceUnderTest.BuildGraph("trasa");
            Console.WriteLine("found " + graph.Users.Count() + " users");
            string result = ServiceUnderTest.BuildGraphML(graph);
            Console.WriteLine("----");
            Console.WriteLine(result);
            Console.WriteLine("----");
        }
    }
}
