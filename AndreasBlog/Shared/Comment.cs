using System;
using System.ComponentModel.DataAnnotations;

namespace AndreasBlog.Shared
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        public string Text { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string? UserId { get; set; }

        public int BlogPostId { get; set; }

    }
}

