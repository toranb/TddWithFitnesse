using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;
using TddWithFitnesse.Controllers;
using System.Web.Mvc;
using TddWithFitnesse.Models;

namespace TddWithFitnesse.Acceptance
{
    public class RetrievePostByUriColumnFixture : ColumnFixture
    {
        private string title, content, uri;

        public bool Found()
        {
            var controller = new PostController();

            var result = controller.RetrievePostByUri(uri) as ViewResult;

            var post = (Post) result.ViewData.Model;

            return validateReturnedPost(post);
        }

        private bool validateReturnedPost(Post post)
        {
            if (post.ID == 0)
                return false;

            if (post.Title != title)
                return false;

            if (post.Content != content)
                return false;

            if (post.Uri != uri)
                return false;

            return true;
        }
    }
}
