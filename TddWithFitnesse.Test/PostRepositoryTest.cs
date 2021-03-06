﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TddWithFitnesse.Repository;
using TddWithFitnesse.Models;
using System.Data.SqlClient;

namespace TddWithFitnesse.Test
{
    [TestClass]
    public class PostRepositoryTest
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        [TestInitialize]
        public void CreateConnectionAndTransaction()
        {
            connection = new SqlConnection(@"server=core2duo\SQLEXPRESS;DATABASE=dsmtbillup;Trusted_Connection=Yes");
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        [TestCleanup]
        public void RollbackTransactionAndCloseConnection()
        {
            transaction.Rollback();
            transaction.Dispose();
            connection.Close();
            connection.Dispose();
        }

        [TestMethod]
        public void ShouldCreateAndRetrievePost()
        {
            var repository = new PostRepository(connection, transaction);

            var post = new Post{ Title = "test", Content = "empty", Uri = "archive/2009/01/01/hello" };

            repository.InsertPost(post);

            var retrievedPost = repository.GetPostByUri(post.Uri);

            Assert.AreEqual(retrievedPost.Title, post.Title);
            Assert.AreEqual(retrievedPost.Content, post.Content);
            Assert.AreEqual(retrievedPost.Uri, post.Uri);
        }
    }
}
