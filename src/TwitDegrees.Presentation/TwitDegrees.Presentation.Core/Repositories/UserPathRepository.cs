using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blackfin.Core.Extensions;
using Blackfin.Core.NHibernate;
using log4net;
using NHibernate.Criterion;
using TwitDegrees.Presentation.Core.Models;

namespace TwitDegrees.Presentation.Core.Repositories
{
    public interface IUserPathRepository
    {
        FriendDictionary GetFriendsOf(string user);
    }
    
    public class UserPathRepository : Repository<TwitterUser>, IUserPathRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserPathRepository(NHibernateSession session) : base(session)
        {
        }

        public FriendDictionary GetFriendsOf(string user)
        {
            IList<UserFriends> friends = Session.CreateCriteria(typeof(UserFriends))
                .Add(Expression.Eq("UserName", user))
                .List().ToListOf<UserFriends>();

            return new FriendDictionary(friends.Select(f => f.IsFriendOf));
        }
    }
}
