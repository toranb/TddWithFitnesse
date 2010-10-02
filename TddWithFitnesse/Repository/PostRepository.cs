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

        public Post GetPostByUri(string uri)
        {
            var post = new Post();

            using (var cmd = new SqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(new SqlParameter("@uri",SqlDbType.VarChar) { Value = uri });
                cmd.CommandText = "select id, title, content, uri from posts where uri = @uri";
                cmd.Connection = new SqlConnection(connectionString);
                cmd.Connection.Open();

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