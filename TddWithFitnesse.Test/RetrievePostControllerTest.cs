using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TddWithFitnesse.Controllers;
using System.Web.Mvc;
using TddWithFitnesse.Models;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class RetrievePostControllerTest
    {
        [TestMethod]
        public void RetrievePostWithValidUri()
        {
            var controller = new PostController();

            var post = new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };

            var result = controller.RetrievePostByUri(post.Uri) as ViewResult;

            var persistedPost = (Post)result.ViewData.Model;

            Assert.AreEqual(persistedPost.Title, post.Title);
            Assert.AreEqual(persistedPost.Content, post.Content);
            Assert.AreEqual(persistedPost.Uri, post.Uri);

        }
    }
}
