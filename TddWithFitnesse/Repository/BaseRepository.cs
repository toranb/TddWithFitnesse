using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TddWithFitnesse.TransactionHelper;

namespace TddWithFitnesse.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        private SqlConnection Connection;
        private SqlTransaction Transaction;

        public BaseRepository() { }

        public BaseRepository(SqlConnection connection, SqlTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        protected SqlTransaction GetSqlTransaction()
        {
            return Transaction == null ? (SqlTransaction)CommonSessionManager.Transaction : Transaction;
        }

        protected SqlConnection GetSqlConnection()
        {
            return Connection == null ? (SqlConnection)CommonSessionManager.Connection : Connection;
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

                return (int)paramList[0].Value;
            }
        }
    }
}