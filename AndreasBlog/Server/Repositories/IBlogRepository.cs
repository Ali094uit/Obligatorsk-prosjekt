using System;
using AndreasBlog.Shared;

namespace AndreasBlog.Server.Repositories
{
	public interface IBlogRepository
	{
		//Blog
		Task<List<BlogPost>?> GetAllBlogPosts();

		Task<BlogPost?> GetBlogPost(string title);

		Task<BlogPost?> GetBlogPostById(int Id);

		Task<BlogPost?> CreateBlogPost(BlogPost request);

		Task<List<BlogPost>> SearchBlogs(string searchText);

		Task<BlogPost?> DeleteBlogPost(int Id);

        Task Like(int Id);

        //Bruker
        Task<User?> CreateUser(User user);

        Task<string?> Login(string username, string password);

        //Kommentar
        Task<Comment?> CreateComment(Comment comment);
		Task<List<Comment?>> GetComments(int postId);
	}
}

