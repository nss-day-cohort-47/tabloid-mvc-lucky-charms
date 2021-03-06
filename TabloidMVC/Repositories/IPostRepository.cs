using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetSubbedPosts(int currentUserId);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        public List<Post> GetAllPostsByUser(int userProfileId);
        public void DeletePost(int id);
        public void EditPost(Post post);
    }
}