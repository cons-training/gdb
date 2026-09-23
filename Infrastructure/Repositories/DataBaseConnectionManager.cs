using System.Data.SqlClient;

namespace GDB.App.Infrastructure.Repositories
{
    public class DataBaseConnectionManager
    {
        private const string _connectionString =
        "Server=localhost,14333;Database=GDBDatabase;User ID=sa;Password=password;TrustServerCertificate=True;";


        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            return connection;
        }
    }
}
