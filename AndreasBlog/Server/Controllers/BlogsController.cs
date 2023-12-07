using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndreasBlog.Server.Data;
using AndreasBlog.Server.Repositories;
using AndreasBlog.Server.Services;
using AndreasBlog.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blazored.LocalStorage;
using System.Security.Claims;

namespace AndreasBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepository blogRepository;

        public BlogsController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogPost>>> GetAllPosts()
        {
            return Ok(await blogRepository.GetAllBlogPosts());
        }

        [HttpGet("{postId}/comments")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForPost(int postId)
        {
            var comments = await blogRepository.GetComments(postId);
            return Ok(comments);
        }

        [HttpDelete("{postId}/delete")]
        public async Task<ActionResult<BlogPost>> DeleteBlogPost(int postId)
        {
            var deletedBlogPost = await blogRepository.DeleteBlogPost(postId);

            if (deletedBlogPost == null)
            {
                // BlogPost med angitt ID ble ikke funnet
                return NotFound();
            }

            // BlogPost ble slettet, returner den slettede ressursen
            return Ok(deletedBlogPost);
        }

        [HttpPut("{postId}/like")]
        public async Task<ActionResult> Like(int postId)
        {
            try
            {
                await blogRepository.Like(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "En feil oppstod ved forsøk på å øke likerantallet.");
            }
        }

        [HttpGet("userpage/{Id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID");
            }

            var user = await blogRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }



        [HttpGet("{title}")]
        public async Task<ActionResult<BlogPost>> GetPost(string title)
        {
            return Ok(await blogRepository.GetBlogPost(title));
        }

        [HttpGet("post/{id}", Name = "GetPostById")]
        public async Task<ActionResult<BlogPost>> GetPostById(int Id)
        {
            return Ok(await blogRepository.GetBlogPostById(Id));
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<BlogPost>>> GetSearchResult(string searchText)
        {
            return Ok(await blogRepository.SearchBlogs(searchText));
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreatePost(BlogPost request)
        {
            return Ok(await blogRepository.CreateBlogPost(request));
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            return Ok(await blogRepository.CreateUser(user));
        }


        [HttpPost("createComment/{id}")]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] Comment comment)
        {
            return Ok(await blogRepository.CreateComment(comment));
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestModel loginModel)
        {
            var jwtToken = await blogRepository.Login(loginModel.UserName, loginModel.Password);
            Console.WriteLine(jwtToken);
            if (!string.IsNullOrEmpty(jwtToken))
            {

                return Ok(jwtToken);
            }

            // Håndter feil eller ugyldige innloggingsopplysninger
            return BadRequest(jwtToken);
        }
    }

}

