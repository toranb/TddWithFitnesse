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
    public class PostRepository : BaseRepository<Post>
    {
        public PostRepository() { }

        public PostRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction) {}

        public Post GetPostByUri(string uri)
        {
            var param = new SqlParameter("@uri", SqlDbType.VarChar) { Value = uri };
            return ExecuteReader("select id, title, content, uri from posts where uri = @uri", param);
        }

        public override Post BuildEntity(SqlCommand cmd)
        {
            var post = new Post();
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

            return post;
        }

        public void InsertPost(Post post)
        {
            post.ID = ExecuteNonQueryWithReturnValue(BuildInsertSql(post), BuildParameterListForInsert(post));
        }

        private string BuildInsertSql(Post post)
        {
            var insertSql = new StringBuilder();
            insertSql.Append("insert into posts (Title,Content,Uri) values (@title,@content,@uri);");
            insertSql.Append("SET @PostID = SCOPE_IDENTITY()");

            return insertSql.ToString();
        }

        private List<SqlParameter> BuildParameterListForInsert(Post post)
        {
            var paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@PostID", SqlDbType.Int) { Direction = ParameterDirection.Output });
            paramList.Add(new SqlParameter("@title", SqlDbType.VarChar) { Value = post.Title });
            paramList.Add(new SqlParameter("@content", SqlDbType.VarChar) { Value = post.Content });
            paramList.Add(new SqlParameter("@uri", SqlDbType.VarChar) { Value = post.Uri });

            return paramList;
        }
    }
}