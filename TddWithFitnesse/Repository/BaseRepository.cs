using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace TddWithFitnesse.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        private string connectionString = @"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes";
        private SqlConnection connection;
        private SqlTransaction transaction;
        private SqlConnection internalConnection;
        private SqlTransaction internalTransaction;

        public BaseRepository() { }

        public BaseRepository(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        protected SqlTransaction GetSqlTransaction()
        {
            if (transaction == null)
            {
                if (internalTransaction == null)
                {
                    internalTransaction = GetSqlConnection().BeginTransaction();
                }
                return internalTransaction;
            }

            return transaction;
        }

        protected SqlConnection GetSqlConnection()
        {
            if (connection == null)
            {
                if (internalConnection == null)
                {
                    internalConnection = new SqlConnection(connectionString);
                    internalConnection.Open();
                }
                return internalConnection;
            }

            return connection;
        }

        protected void CommitTransactionWhenSqlConnectionIsOpen()
        {
            if (transaction == null)
            {
                internalTransaction.Commit();
            }
        }

        public abstract T BuildEntity(SqlCommand cmd);

        public T ExecuteReader(string commandText, SqlParameter param)
        {
            using (var cmd = GetSqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(param);
                cmd.CommandText = commandText;
                cmd.Connection = GetSqlConnection();
                cmd.Transaction = GetSqlTransaction();

                return this.BuildEntity(cmd);
            }
        }

        public int ExecuteNonQueryWithReturnValue(string commandText, List<SqlParameter> paramList)
        {
            using (SqlCommand cmd = GetSqlConnection().CreateCommand())
            {
                foreach (SqlParameter param in paramList)
                {
                    cmd.Parameters.Add(param);
                }
                cmd.CommandText = commandText;
                cmd.Connection = GetSqlConnection();
                cmd.Transaction = GetSqlTransaction();

                cmd.ExecuteNonQuery();

                CommitTransactionWhenSqlConnectionIsOpen();

                return (int)paramList[0].Value;
            }
        }
    }
}