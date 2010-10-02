using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TddWithFitnesse.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace TddWithFitnesse.Repository
{
    public class PostRepository
    {
        private string connectionString = @"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes";
        private SqlConnection connection;
        private SqlTransaction transaction;
        private SqlConnection internalConnection;
        private SqlTransaction internalTransaction;

        public PostRepository() { }

        public PostRepository(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        private SqlTransaction GetSqlTransaction()
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

        private SqlConnection GetSqlConnection()
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

        private void CommitTransactionWhenSqlConnectionIsOpen()
        {
            if (transaction == null)
            {
                internalTransaction.Commit();
            }
        }

        public Post GetPostByUri(string uri)
        {
            var post = new Post();

            using (var cmd = GetSqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(new SqlParameter("@uri",SqlDbType.VarChar) { Value = uri });
                cmd.CommandText = "select id, title, content, uri from posts where uri = @uri";
                cmd.Connection = GetSqlConnection();
                cmd.Transaction = GetSqlTransaction();

                BuildPostObject(post, cmd);
            }
            return post;
        }

        private void BuildPostObject(Post post, SqlCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader != null)
                    while (reader.Read())
                    {
                        post.ID = Convert.ToInt32(reader["ID"]);
                        post.Title = Convert.ToString(reader["Title"]);
                        post.Uri = Convert.ToString(reader["uri"]);
                        post.Content = Convert.ToString(reader["Content"]);
                    }
            }
        }

        public void InsertPost(Post post)
        {
            var identityValue = BuildIdentityValueParameter();
            using (SqlCommand cmd = GetSqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(identityValue);
                AddParameterValuesForInsert(cmd, post);
                cmd.CommandText = BuildInsertSql(post);
                cmd.Connection = GetSqlConnection();
                cmd.Transaction = GetSqlTransaction();

                cmd.ExecuteNonQuery();

                post.ID = (int)identityValue.Value;
            }

            CommitTransactionWhenSqlConnectionIsOpen();
        }

        private string BuildInsertSql(Post post)
        {
            var insertSql = new StringBuilder();
            insertSql.Append("insert into posts (Title,Content,Uri) values (@title,@content,@uri);");
            insertSql.Append("SET @PostID = SCOPE_IDENTITY()");

            return insertSql.ToString();
        }

        private void AddParameterValuesForInsert(SqlCommand cmd, Post post)
        {
            cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar) { Value = post.Title });
            cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarChar) { Value = post.Content });
            cmd.Parameters.Add(new SqlParameter("@uri", SqlDbType.VarChar) { Value = post.Uri });
        }

        private SqlParameter BuildIdentityValueParameter()
        {
            return new SqlParameter("@PostID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        }
    }
}