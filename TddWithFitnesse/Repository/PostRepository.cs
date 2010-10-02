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

        public void InsertPost(Post post)
        {
            var identityValue = BuildIdentityValueParameter();
            using (SqlCommand cmd = new SqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(identityValue);
                AddParameterValuesForInsert(cmd, post);
                cmd.CommandText = BuildInsertSql(post);
                cmd.Connection = new SqlConnection(connectionString);
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                post.ID = (int)identityValue.Value;
            }
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