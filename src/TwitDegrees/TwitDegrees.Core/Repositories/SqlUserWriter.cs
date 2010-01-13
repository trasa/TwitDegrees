using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Core.Repositories
{
    public interface IUserWriter
    {
        void WriteFriends(string user, IEnumerable<UserInfo> friends);
        void WriteUserInfo(UserInfo info);
        void WriteFollowers(string userName, IEnumerable<UserInfo> followers);
    }

    public class SqlUserWriter : IUserWriter
    {
        public void WriteFriends(string user, IEnumerable<UserInfo> friends)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");

            using(var conn = ConnectionFactory.Create())
            {
                conn.Open();
                using (var cmd = new SqlCommand("addFriend", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@username", user);
                    var friendName = cmd.Parameters.Add("@friendname", SqlDbType.NVarChar);
                    var location = cmd.Parameters.Add("@location", SqlDbType.NVarChar);
                    var friendCount = cmd.Parameters.Add("@friendCount", SqlDbType.Int);
                    var followerCount = cmd.Parameters.Add("@followerCount", SqlDbType.Int);

                    foreach(var friend in friends)
                    {
                        if (string.IsNullOrEmpty(friend.Name))
                        {
                            // weird, well can't have null as a friend, so skip it.
                            break;
                        }
                        friendName.Value = friend.Name;
                        location.Value = friend.Location;
                        friendCount.Value = friend.FriendCount;
                        followerCount.Value = friend.FollowerCount;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void WriteFollowers(string user, IEnumerable<UserInfo> followers)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");

            using (var conn = ConnectionFactory.Create())
            {
                conn.Open();
                using (var cmd = new SqlCommand("addFriend", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    // note that this is the same call to AddFriend, except that
                    // userName and friendName have been reversed.

                    cmd.Parameters.AddWithValue("@friendname", user);
                    var userName = cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                    var location = cmd.Parameters.Add("@location", SqlDbType.NVarChar);
                    var friendCount = cmd.Parameters.Add("@friendCount", SqlDbType.Int);
                    var followerCount = cmd.Parameters.Add("@followerCount", SqlDbType.Int);

                    foreach (var follower in followers)
                    {
                        if (string.IsNullOrEmpty(follower.Name))
                        {
                            // weird, well can't have null as a friend, so skip it.
                            break;
                        }
                        userName.Value = follower.Name;
                        location.Value = follower.Location;
                        friendCount.Value = follower.FriendCount;
                        followerCount.Value = follower.FollowerCount;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void WriteUserInfo(UserInfo info)
        {
            using (var conn = ConnectionFactory.Create())
            {
                conn.Open();
                using(var cmd = new SqlCommand("AddTwitterUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", info.Name);
                    cmd.Parameters.AddWithValue("@location", info.Location);
                    cmd.Parameters.AddWithValue("@friendCount", info.FriendCount);
                    cmd.Parameters.AddWithValue("@followerCount", info.FollowerCount);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
