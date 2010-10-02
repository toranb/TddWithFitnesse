using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using TddWithFitnesse.Controllers;
using TddWithFitnesse.Models;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class CreatePostControllerTest
    {
        [TestMethod]
        public void CreatePostIsSuccessfulWithValidData()
        {
            var controller = new PostController();

            var post = new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };

            var result = controller.CreatePost(post) as ViewResult;

            var insertedPost = (Post) result.ViewData.Model;

            Assert.IsTrue(insertedPost.ID > 0);
        }
    }
}
