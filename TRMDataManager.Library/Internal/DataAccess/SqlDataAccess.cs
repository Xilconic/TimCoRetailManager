using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            // TODO: This method should be private. Going along with course...
            // TODO: Unsure about implicitly assuming the parent library is going to use ConfigurationManager. Going along with course...
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<TOutput> LoadData<TOutput, TParameters>(
            string storedProcedure,
            TParameters parameters,
            string connectionStringName)
        {
            // TODO: DRY - Duplicate code pattern in SaveData; Going along with course...
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<TOutput> rows = connection.Query<TOutput>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure)
                    .ToList();

                // TODO: Violates recommended practice in C#. Should return as interface.
                //       Going along with course...
                return rows;
            }
        }

        public void SaveData<TParameters>(
            string storedProcedure,
            TParameters parameters,
            string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}