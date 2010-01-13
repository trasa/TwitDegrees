using System.Collections.Generic;
using TwitDegrees.Presentation.Core.Models;
using TwitDegrees.Presentation.Core.Repositories;

namespace TwitDegrees.Presentation.Core.Services
{
    public interface IPathService
    {
        IEnumerable<string> FindPath(string sourceUser, string destinationUser);
    }

    public class PathService : IPathService
    {
        private readonly IUserPathRepository userPathRepository;

        public PathService(IUserPathRepository userPathRepository)
        {
            this.userPathRepository = userPathRepository;
        }

        public IEnumerable<string> FindPath(string sourceUser, string destinationUser)
        {
            if (sourceUser == destinationUser)
                return new List<string> {sourceUser};

            var friendsVisited = new UserVisitedList();
            return Visit(sourceUser, destinationUser, friendsVisited);
            
        }

        private List<string> Visit(string sourceUser, string destinationUser, UserVisitedList visited)
        {
            visited.Visit(sourceUser);
            FriendDictionary friendsOfSource = userPathRepository.GetFriendsOf(sourceUser);
            if (friendsOfSource.Contains(destinationUser))
            {
                // we found it.
                return new List<string> { sourceUser, destinationUser };
            }
            // get highest-friendcount friend (that we have not yet visited) and see if that leads to the destination.
            // if not, try the Nth friend, until there are no more friends, then give up on this branch.
            foreach(TwitterUser friend in friendsOfSource.Friends)
            {
                if (!visited.Visited(friend.Name))
                {
                    List<string> searchResult = Visit(friend.Name, destinationUser, visited);
                    if (searchResult != null)
                    {
                        // this branch was successful.  searchResult already contains friend.Name, so append our name to the head of the list.
                        searchResult.Insert(0, sourceUser);
                        return searchResult;
                    }
                }
            }
            // this branch was not successful, no friends of sourceUser matched destinationUser.
            return new List<string>();
        }
    }
}
