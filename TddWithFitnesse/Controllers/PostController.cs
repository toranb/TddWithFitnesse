using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TddWithFitnesse.Models;
using System.Data.SqlClient;
using System.Data;

namespace TddWithFitnesse.Controllers
{
    public class PostController : Controller
    {
        private string connectionString = @"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes";
        public ViewResult CreatePost(Post post)
        {
            var identityValue = new SqlParameter("@PostID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            using (SqlCommand cmd = new SqlConnection().CreateCommand())
            {
                cmd.Parameters.Add(identityValue);
                cmd.CommandText = "insert into posts (Title, Content, Uri) values ('" + post.Title + "','" + post.Content + "','" + post.Uri + "');SET @PostID = SCOPE_IDENTITY()";
                cmd.Connection = new SqlConnection(connectionString);
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                post.ID = (int)identityValue.Value;
            }

            return View("CreatePost", post);
        }
    }
}
