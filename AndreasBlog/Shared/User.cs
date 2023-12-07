using System;
using Microsoft.AspNetCore.Identity;

namespace AndreasBlog.Shared

{
    public class User : IdentityUser
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}

