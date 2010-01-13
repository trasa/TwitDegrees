using System.Collections.Generic;
using Blackfin.Core.Collections;


namespace TwitDegrees.Presentation.Core.Models
{
    public class UserGraph
    {
        private readonly Dictionary<string, string> nodes = new Dictionary<string, string>();
        private readonly ListDictionary<string, string> edges = new ListDictionary<string, string>();
        
        public IEnumerable<string> Users
        {
            get
            {
                return nodes.Values;
            }
        }

        public IEnumerable<Edge> Edges
        {
            get
            {
                foreach(var pair in edges)
                {
                    foreach(var target in pair.Value)
                    {
                        yield return new Edge(pair.Key, target);
                    }
                }
            }
        }

        public void Add(string source, string target)
        {
            AddNode(source);
            AddNode(target);
            edges.Add(source, target);
        }

        private void AddNode(string name)
        {
            if (!nodes.ContainsKey(name))
                nodes.Add(name, name);
        }
    }

    public class Edge
    {
        public Edge(string source, string target)
        {
            Source = source;
            Target = target;
        }

        public string Source { get; private set; }
        public string Target { get; private set; }
    }
}
