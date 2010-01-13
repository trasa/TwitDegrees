using System.Collections.Generic;

namespace TwitDegrees.Presentation.Core.Services
{
    public class UserVisitedList
    {
        private readonly Dictionary<string, string> visited = new Dictionary<string, string>();

        public void Visit(string userName)
        {
            visited.Add(userName, userName);
        }

        public bool Visited(string userName)
        {
            return visited.ContainsKey(userName);
        }
    }
}
