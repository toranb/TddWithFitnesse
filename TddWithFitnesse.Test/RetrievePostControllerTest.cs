using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TddWithFitnesse.Controllers;
using System.Web.Mvc;
using TddWithFitnesse.Models;
using TddWithFitnesse.Repository;
using Moq;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class RetrievePostControllerTest
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
        public void RetrievePostWithValidUri()
        {
            var post = new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };
            fakePostRepository.Setup(x => x.GetPostByUri(post.Uri)).Returns(post);

            var result = controller.RetrievePostByUri(post.Uri) as ViewResult;

            var persistedPost = (Post)result.ViewData.Model;

            Assert.AreEqual(persistedPost.Title, post.Title);
            Assert.AreEqual(persistedPost.Content, post.Content);
            Assert.AreEqual(persistedPost.Uri, post.Uri);
            fakePostRepository.Verify(x => x.GetPostByUri(post.Uri));
        }

        [TestMethod]
        public void WhenUriIsNotFoundReturnNewPostObject()
        {
            var uri = "2009/01/01/magic";
            fakePostRepository.Setup(x => x.GetPostByUri(uri)).Returns(new Post());

            var result = controller.RetrievePostByUri(uri) as ViewResult;

            var post = (Post)result.ViewData.Model;

            Assert.AreEqual(post.ID, 0);
            Assert.AreEqual(post.Title, null);
            Assert.AreEqual(post.Content, null);
            Assert.AreEqual(post.Uri, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "invalid uri")]
        public void WhenUriParamIsNullThrowException()
        {
            var result = controller.RetrievePostByUri("") as ViewResult;
        }
    }
}
