using System;
using AndreasBlog.Server.Data;
using AndreasBlog.Server.Services;
using AndreasBlog.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AndreasBlog.Server.Services;

namespace AndreasBlog.Server.Repositories
{
    public class BlogRepository : IBlogRepository
	{
        private readonly AppDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtService iJWTService;

        public BlogRepository(AppDbContext appDbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService iJWTService)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.iJWTService = iJWTService;
        }

        public async Task<BlogPost?> CreateBlogPost(BlogPost request)
        {
            appDbContext.BlogPosts.Add(request);
            await appDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<List<BlogPost>?> GetAllBlogPosts()
        {
            return await appDbContext.BlogPosts.OrderByDescending(post => post.DateCreated).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPost(string title)
        {
            return await appDbContext.BlogPosts.FirstOrDefaultAsync(p => p.Title.ToLower().Equals(title.ToLower()));
        }

        public async Task<BlogPost?> GetBlogPostById(int Id)
        {
            return await appDbContext.BlogPosts.FirstOrDefaultAsync(p => p.id.Equals(Id));
        }

        public async Task<List<BlogPost>> SearchBlogs(string searchText)
        {
            return await appDbContext.BlogPosts.Where(p => p.Title.ToLower().Contains(searchText.ToLower())).ToListAsync();
        }

        public async Task<List<Comment>> GetComments(int postId)
        {
            return await appDbContext.Comments.Where(c => c.BlogPostId == postId).ToListAsync();
        }


        public async Task<Comment?> CreateComment(Comment comment)
        {
            // Sjekk om blogginnlegget og brukeren eksisterer i databasen
            var existingBlogPost = await appDbContext.BlogPosts.FindAsync(comment.BlogPostId);
            var existingUser = await appDbContext.Users.FindAsync(comment.UserId);

            if (existingBlogPost == null || existingUser == null)
            {
                // Blogginnlegg eller bruker eksisterer ikke
                return null;
            }

            appDbContext.Comments.Add(comment);
            await appDbContext.SaveChangesAsync();
            return comment;
        }


        public async Task<string?> Login(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result != null)
                {
                    // Innloggingen var vellykket, generer JWT-token og send tilbake
                    var token = iJWTService.GenerateToken(user);
                    return token;
                }
            }

           
            return null;
        }


        public async Task<User?> CreateUser(User user)
        {
            
            var result = await userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return user;
            }

            //Ved feil ->
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (null);
        }

        public async Task<BlogPost?> DeleteBlogPost(int id)
        {
            var blogPostToDelete = await appDbContext.BlogPosts.FindAsync(id);

            if (blogPostToDelete == null)
            {
                // BlogPost ikke funnet
                return null;
            }

            appDbContext.BlogPosts.Remove(blogPostToDelete);
            await appDbContext.SaveChangesAsync();

            return blogPostToDelete;
        }

        public async Task Like(int Id)
        {
            var blogPost = await appDbContext.BlogPosts.FindAsync(Id);

            if(blogPost == null)
            {
                return;
            }

            blogPost.Likes += 1;

            await appDbContext.SaveChangesAsync();

        }

        public async Task<User?> GetUserById(string id)
        {
            return await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> UpdateUserInfo(string id, string email, string phoneNumber)
        {
            try
            {
                
                var userToUpdate = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (userToUpdate != null)
                {
                    
                    userToUpdate.Email = email;
                    userToUpdate.PhoneNumber = phoneNumber;

                    
                    await appDbContext.SaveChangesAsync();
                    return userToUpdate;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Feil ved oppdatering i databasen: {ex.Message}");
                return null;
            }
        }
    }
}

