using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
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

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void SaveDataInTransaction<TParameters>(
            string storedProcedure,
            TParameters parameters)
        {
            // TODO: DRY - similar code pattern in SaveData; Going along with course...
            _connection.Execute(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<TOutput> LoadDataInTransaction<TOutput, TParameters>(
            string storedProcedure,
            TParameters parameters)
        {
            // TODO: DRY - Duplicate code pattern in LoadData; Going along with course...
            List<TOutput> rows = _connection.Query<TOutput>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: _transaction)
                .ToList();

            // TODO: Violates recommended practice in C#. Should return as interface. Going along with course...
            return rows;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close(); // TODO: DRY, repeated in CommitTransaction; Going along with course
        }

        // Open connect/start transaction method
        // load using transaction
        // save using transatoin
        // Close connection/start transaction method
        // Dispose

        public void Dispose()
        {
            // TODO: This doesn't implement Microsoft recommend Dispose pattern; Going along with course...
            CommitTransaction();
        }
    }
}