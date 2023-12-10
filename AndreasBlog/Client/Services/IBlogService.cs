using System;
using AndreasBlog.Shared;

namespace AndreasBlog.Client.Services
{
	public interface IBlogService
	{
        Task<List<BlogPost>?> GetAllBlogPosts();

        Task<BlogPost?> GetBlogPost(string title);

        Task<BlogPost?> CreateBlogPost(BlogPost request);

        Task<List<BlogPost>> SearchBlogs(string searchText);

        Task<BlogPost?> GetBlogPostById(int? Id);

        Task<bool> DeleteBlogPost(int? Id);

        Task<bool> Like(int Id);

        //Bruker
        Task<User?> CreateUser(User user);

        Task<User?> GetUserById(string Id);

        Task<string?> LoginUser(string username, string password);

        Task<User> UpdateUserInfo(User user);

        //Kommentar
        Task<Comment?> CreateComment(Comment comment);

        Task<List<Comment>> GetComments(int postId);

        void AddAuthorizationHeader(string scheme, string token);

    }
}

