using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;
using TddWithFitnesse.Models;
using TddWithFitnesse.Controllers;
using System.Web.Mvc;

namespace TddWithFitnesse.Acceptance
{
    public class GivenPostWithValidUriColumnFixture : ColumnFixture
    {
        private string title, content, uri;

        public bool Create()
        {
            var post = new Post { Title = title, Content = content, Uri = uri };

            var controller = new PostController();

            var result = controller.CreatePost(post) as ViewResult;

            var createdPost = (Post) result.ViewData.Model;

            return createdPost.ID > 0;
        }
    }
}
