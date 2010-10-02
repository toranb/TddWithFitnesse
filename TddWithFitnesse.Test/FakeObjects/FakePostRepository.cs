using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TddWithFitnesse.Repository;
using TddWithFitnesse.Models;

namespace TddWithFitnesse.Test.FakeObjects
{
    public class FakePostRepository : PostRepository
    {
        public bool insertWasCalled;
        public bool retrieveWasCalled;

        public override void InsertPost(Post post)
        {
            insertWasCalled = true;
        }

        public override Post GetPostByUri(string uri)
        {
            retrieveWasCalled = true;
            return new Post() { Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };
        }
    }
}
