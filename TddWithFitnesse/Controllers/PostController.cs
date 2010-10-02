using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TddWithFitnesse.Models;
using System.Data.SqlClient;
using System.Data;
using TddWithFitnesse.Repository;

namespace TddWithFitnesse.Controllers
{
    public class PostController : Controller
    {
        private PostRepository repository;

        public PostController() : this(new PostRepository()) { }

        public PostController(PostRepository repository) 
        {
            this.repository = repository;
        }

        public ViewResult CreatePost(Post post)
        {
            repository.InsertPost(post);

            return View("CreatePost", post);
        }

        public ViewResult RetrievePostByUri(string uri)
        {
            var post = repository.GetPostByUri(uri);

            return View("GetPostByUri", post);
        }
    }
}
