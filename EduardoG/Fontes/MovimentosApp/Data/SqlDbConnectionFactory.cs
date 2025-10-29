using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MovimentosApp.Data
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlDbConnectionFactory(IConfiguration cfg)
        {
            _connectionString = cfg.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
