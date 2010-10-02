using System;
namespace TddWithFitnesse.Repository
{
    public interface IPostRepository
    {
        TddWithFitnesse.Models.Post BuildEntity(System.Data.SqlClient.SqlCommand cmd);
        TddWithFitnesse.Models.Post GetPostByUri(string uri);
        void InsertPost(TddWithFitnesse.Models.Post post);
    }
}
