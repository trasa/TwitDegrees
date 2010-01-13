using System.Data;
using System.Data.SqlClient;
using System.Xml;
using TwitDegrees.Presentation.Core.Models;

namespace TwitDegrees.Presentation.Core.Services
{
    public interface IGraphService
    {
        UserGraph BuildGraph(string rootUser);
        string BuildGraphML(string rootUser);
        string BuildGraphML(UserGraph graphData);
    }

    public class GraphService : IGraphService
    {

        public UserGraph BuildGraph(string rootUser)
        {
            var graph = new UserGraph();

            // HACK!
            using (var conn = ConnectionFactory.Create())
            {
                conn.Open();
                // force things to only 2 levels...
                // trasa has 193 friends (1st degree)
                // those friends have 248,634 friends (2nd degree contacts)
                // those friends have well over 12 million friends (3rd degree, and the query times out before it can finish)
                #region sql = "Select..";
                const string sql = @"select UserName, IsFriendOf, 1 as Degree from userfriends where UserName = @user
UNION
select UserName, IsFriendOf, 2 as Degree 
from userfriends 
where UserName in (select IsFriendOf from userfriends where UserName = @user)";
                #endregion

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", rootUser);
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            graph.Add(dr.GetString(0), dr.GetString(1));
                        }
                    }
                }
                return graph;
            }
        }

        public string BuildGraphML(string rootUser)
        {
            UserGraph graph = BuildGraph(rootUser);
            return BuildGraphML(graph);
        }

        public string BuildGraphML(UserGraph graphData)
        {
            var doc = new XmlDocument();
            var graphml = doc.CreateElement("graphml", "http://graphml.graphdrawing.org/xmlnls");
            doc.AppendChild(graphml);
            
            var graph = doc.CreateElement("graph");
            graphml.AppendChild(graph);

            var graphAttr = doc.CreateAttribute("edgedefault");
            graphAttr.Value = "unidirected";
            graph.Attributes.Append(graphAttr);

            // keys
            var key = doc.CreateElement("key");
            var attrId = doc.CreateAttribute("id");
            attrId.Value = "name";
            key.Attributes.Append(attrId);

            var attrFor = doc.CreateAttribute("for");
            attrFor.Value = "node";
            key.Attributes.Append(attrFor);

            var attrName = doc.CreateAttribute("attr.name");
            attrName.Value = "name";
            key.Attributes.Append(attrName);

            var attrType = doc.CreateAttribute("attr.type");
            attrType.Value = "string";
            key.Attributes.Append(attrType);

            graph.AppendChild(key);
            
            foreach(string userName in graphData.Users)
            {
                var node = doc.CreateElement("node");
                
                var id = doc.CreateAttribute("id");
                id.Value = userName;
                node.Attributes.Append(id);

                var data = doc.CreateElement("data");
                var k = doc.CreateAttribute("key");
                k.Value = "name";
                data.Attributes.Append(k);
                data.InnerText= userName;
                node.AppendChild(data);

                graph.AppendChild(node);
            }

            foreach(Edge e in graphData.Edges)
            {
                var edge = doc.CreateElement("edge");
                var source = doc.CreateAttribute("source");
                source.Value = e.Source;
                var target = doc.CreateAttribute("target");
                target.Value = e.Target;

                edge.Attributes.Append(source);
                edge.Attributes.Append(target);

                graph.AppendChild(edge);
            }


            return doc.OuterXml;

        }
    }
}
