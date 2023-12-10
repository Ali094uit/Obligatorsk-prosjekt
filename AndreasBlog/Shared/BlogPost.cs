using System;
using System.ComponentModel.DataAnnotations;

namespace AndreasBlog.Shared
{
	public class BlogPost
	{
		public int id { get; set; }

        [Required(ErrorMessage = "Tittel er påkrevd"), StringLength(50, ErrorMessage = "Maks tekstlengde 50")]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Beskrivelse er påkrevd")]
        public string Description { get; set; } = string.Empty;

		public string? Author { get; set; }

		public string? Image { get; set; }

		public DateTime DateCreated { get; set; } = DateTime.Now;

		public bool isPublished { get; set; } = true;

		public bool isDeleted { get; set; } = false;

        public string UserId { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public int Likes { get; set; } = 0;
    }
}

