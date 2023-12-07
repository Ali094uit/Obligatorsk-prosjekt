using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AndreasBlog.Shared;

namespace AndreasBlog.Client.Services
{
	public class BlogService : IBlogService
	{
        private readonly HttpClient httpClient;

		public BlogService(HttpClient httpClient)
		{
            this.httpClient = httpClient;
		}

        public async Task<BlogPost?> CreateBlogPost(BlogPost request)
        {
            var result = await httpClient.PostAsJsonAsync("api/Blogs", request);

            if (!result.IsSuccessStatusCode)
            {
                // Log or handle the error appropriately
                Console.WriteLine($"HTTP request failed with status code: {result.StatusCode}");
                return null;
            }

            var jsonContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response JSON: {jsonContent}");

            return await result.Content.ReadFromJsonAsync<BlogPost>();
        }

        public async Task<List<BlogPost>?> GetAllBlogPosts()
        {
            return await httpClient.GetFromJsonAsync<List<BlogPost>>("api/Blogs");
        }

        public async Task<BlogPost?> GetBlogPost(string title)
        {
            var result = await httpClient.GetAsync($"api/Blogs/{title}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                return new BlogPost { Title = message };
            }

            var blogPost = await result.Content.ReadFromJsonAsync<BlogPost>();

            // Hent også kommentarene for dette blogginnlegget
            blogPost.Comments = await GetComments(blogPost.id);

            return blogPost;
        }

        public async Task<List<Comment>> GetComments(int postId)
        {
            var commentsResult = await httpClient.GetAsync($"api/Blogs/{postId}/comments");
            if (commentsResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // Håndter feil her hvis nødvendig
                return new List<Comment>();
            }

            return await commentsResult.Content.ReadFromJsonAsync<List<Comment>>();
        }

        public async Task<List<BlogPost>> SearchBlogs(string searchText)
        {
            return await httpClient.GetFromJsonAsync<List<BlogPost>>($"api/Blogs/Search/{searchText}");
        }

        public async Task<Comment?> CreateComment(Comment comment)
        {
            var result = await httpClient.PostAsJsonAsync("api/Blogs/createComment/{id}", comment);
            return await result.Content.ReadFromJsonAsync<Comment>();
        }

        public async Task<string?> LoginUser(string username, string password)
        {

            var result = await httpClient.PostAsJsonAsync("api/Blogs/Login", new { username, password });

            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.Content);
                return await result.Content.ReadAsStringAsync();
            }

            else
            {
                var errorContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP request failed with content: {errorContent}");
            }

            return null;
        }


        public async Task<User?> CreateUser(User user)
        {
            var result = await httpClient.PostAsJsonAsync("api/Blogs/createUser", user);
            return await result.Content.ReadFromJsonAsync<User?>();
        }

        public void AddAuthorizationHeader(string scheme, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }

        public async Task<BlogPost?> GetBlogPostById(int? Id)
        {
            var result = await httpClient.GetAsync($"api/Blogs/post/{Id}");

            if (result.IsSuccessStatusCode)
            {
                // Request was successful, deserialize and return the BlogPost
                return await result.Content.ReadFromJsonAsync<BlogPost>();
            }
            else
            {
                // Request failed, return a BlogPost with the error message
                var message = await result.Content.ReadAsStringAsync();
                return new BlogPost { Title = message };
            }
        }

        public async Task<bool> DeleteBlogPost(int? Id)
        {
            if (Id == null)
            {
                return false;
            }

            var result = await httpClient.DeleteAsync($"api/Blogs/{Id}/delete");

            if (result.IsSuccessStatusCode)
            {
                // BlogPost ble slettet vellykket
                return true;
            }
            else
            {
                // BlogPost ble ikke funnet eller det oppstod en feil ved sletting
                Console.WriteLine($"HTTP request failed with status code: {result.StatusCode}");
                return false;
            }
        }

        public async Task<bool> Like(int Id)
        {
            if (Id <= 0)
            {
                return false;
            }

            try
            {
                var response = await httpClient.PutAsync($"api/Blogs/{Id}/like", null);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

