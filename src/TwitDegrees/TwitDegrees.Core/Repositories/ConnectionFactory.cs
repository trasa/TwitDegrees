using System.Configuration;
using System.Data.SqlClient;

namespace TwitDegrees.Core.Repositories
{
    public static class ConnectionFactory
    {
        private static readonly string connectionString;

        static ConnectionFactory()
        {
            connectionString = ConfigurationManager.ConnectionStrings["twitdegrees"].ConnectionString;
        }

        public static SqlConnection Create()
        {
            return new SqlConnection(connectionString);
        }
    }
}
