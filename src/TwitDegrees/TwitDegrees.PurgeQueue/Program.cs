using System;
using System.Data;
using System.Data.SqlClient;
using System.Messaging;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Repositories;

namespace TwitDegrees.PurgeQueue
{
    /// <summary>
    /// Delete FriendRequests from the Request queue for users where we've already
    /// gotten friend information.
    /// </summary>
    class Program
    {
        private static SqlConnection connection;
        private static SqlCommand cmd;

        static void Main(string[] args)
        {
            string serverName;
            if (args.Length == 0)
            {
                Console.WriteLine("Assuming trasa1000 as the server name");
                serverName = "trasa1000";
            }
            else
            {
                serverName = args[0];
            }

            string queueName = "FormatName:DIRECT=OS:" + serverName + @"\private$\TwitterRequest";

            using (connection = ConnectionFactory.Create())
            {
                connection.Open();
                cmd = new SqlCommand("FirstFriend", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 255);

                using (var q = new MessageQueue(queueName)
                                   {
                                       MessageReadPropertyFilter = { AppSpecific = true },
                                       Formatter = new BinaryMessageFormatter()
                                   })
                {
                    var cursor = q.CreateCursor();

                    // first one:
                    var m = PeekWithoutTimeout(q, cursor, PeekAction.Current);
                    VerifyMessage(q, m);

                    // loop the rest:
                    while ((m = PeekWithoutTimeout(q, cursor, PeekAction.Next)) != null)
                    {
                        VerifyMessage(q, m);
                    }
                }
            }
        }

        private static void VerifyMessage(MessageQueue q, Message m)
        {
            var request = m.Body as GetFriendsRequest;
            if (request != null && ShouldCancel(request.UserName))
            {
                // this removes the message from the queue,
                // and (apparently) doesn't mess up the queue cursor...
                try
                {
                    q.ReceiveById(m.Id, new TimeSpan(0, 0, 30));
                } 
                catch(Exception)
                {
                    Console.WriteLine("Failed to receive (delete) message for " + request);
                }
            }
        }

        private static bool ShouldCancel(string username)
        {
            if (string.IsNullOrEmpty(username))
                return true;
            cmd.Parameters["@name"].Value = username;
            var result = (string)cmd.ExecuteScalar();
            
            // cancel if there are friends (if there are friends, we already crawled this user recently)
            bool shouldCancel = !string.IsNullOrEmpty(result);
            
            if (shouldCancel)
                Console.WriteLine("Cancel request for " + username);
            
            return shouldCancel;
        }

        private static Message PeekWithoutTimeout(MessageQueue q, Cursor cursor, PeekAction action)
        {
            Message ret = null;
            try
            {
                ret = q.Peek(new TimeSpan(1), cursor, action);
            }
            catch (MessageQueueException mqe)
            {
                if (!mqe.Message.ToLower().Contains("timeout"))
                {
                    throw;
                }
            }
            return ret;
        }
    }
}
