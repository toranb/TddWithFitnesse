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
        private IPostRepository repository;

        public PostController() : this(new PostRepository()) { }

        public PostController(IPostRepository repository) 
        {
            this.repository = repository;
        }

        public ViewResult CreatePost(Post post)
        {
            ThrowExceptionWithNullArgument(post);

            repository.InsertPost(post);

            return View("CreatePost", post);
        }

        private void ThrowExceptionWithNullArgument(Post post)
        {
            if (post == null)
                throw new ArgumentNullException("invalid post");
        }

        public ViewResult RetrievePostByUri(string uri)
        {
            ThrowExceptionWithNullUri(uri);

            var post = repository.GetPostByUri(uri);

            return View("GetPostByUri", post);
        }

        private static void ThrowExceptionWithNullUri(string uri)
        {
            if (String.IsNullOrEmpty(uri))
                throw new ArgumentNullException("invalid uri");
        }
    }
}
