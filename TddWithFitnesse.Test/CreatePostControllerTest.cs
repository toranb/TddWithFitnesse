using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using TddWithFitnesse.Controllers;
using TddWithFitnesse.Models;
using TddWithFitnesse.Repository;
using TddWithFitnesse.Test.FakeObjects;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class CreatePostControllerTest
    {
        [TestMethod]
        public void CreatePostIsSuccessfulWithValidData()
        {
            var fakePostRepository = new FakePostRepository();
            var controller = new PostController(fakePostRepository);

            var post = new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };

            var result = controller.CreatePost(post) as ViewResult;

            Assert.IsTrue(fakePostRepository.insertWasCalled);
        }
    }
}
