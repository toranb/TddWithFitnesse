using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using TddWithFitnesse.Controllers;
using TddWithFitnesse.Models;
using TddWithFitnesse.Repository;
using Moq;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class CreatePostControllerTest
    {
        private Mock<IPostRepository> fakePostRepository;
        private PostController controller;

        [TestInitialize]
        public void Setup()
        {
            fakePostRepository = new Mock<IPostRepository>();
            controller = new PostController(fakePostRepository.Object);
        }

        [TestMethod]
        public void CreatePostIsSuccessfulWithValidData()
        {
            var post = new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };

            var result = controller.CreatePost(post) as ViewResult;

            fakePostRepository.Verify(x => x.InsertPost(post));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "invalid post")]
        public void WhenPostParamIsNullThrowException()
        {
            var result = controller.CreatePost(null);
        }
    }
}
