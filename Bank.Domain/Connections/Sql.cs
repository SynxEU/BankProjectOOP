using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Bank.Domain.Connections
{
    public class Sql
    {
        private IConfigurationRoot _configuration;
        private string? _connectionString;

        /// <summary>
        /// Executes SQL Stored Procedures
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>SqlCommand</returns>
        public SqlCommand Execute(string storedProcedure)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcedure, new SqlConnection(_connectionString));
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
        }

        public Sql()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            _configuration = builder.Build();
            //C:\Users\jonas\source\repos\Bank\Bank\appsettings.json
            _connectionString = _configuration["ConnectionStrings:Default"];
        }

    }
}

