using Microsoft.Data.SqlClient;
using System.Data;

namespace AccotuntsApi.Context
{
    public class DapperContext
    {

        private readonly IConfiguration _configuration;
        private readonly String _connectionString;


        public DapperContext (IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SqlConnection");

        }


        public IDbConnection CreateConnection()=>new SqlConnection( _connectionString);
    }
}
