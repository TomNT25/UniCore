using Microsoft.Data.SqlClient;

namespace UniCore.Infrastructure.Database
{
    public class DatabaseConfiguration
    {
        private string _connectionString = "";

        public DatabaseConfiguration(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateSqlConnection() => new SqlConnection(_connectionString);
        public SqlCommand CreateSqlCommand(string query, SqlConnection conn) => new SqlCommand(query, conn);
    }
}