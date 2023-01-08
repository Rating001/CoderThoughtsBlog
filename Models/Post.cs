using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderThoughtsBlog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Display(Name = "Blog Name")]
        public int BlogId { get; set; }
        public string AuthorId { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Abstract { get; set; }
        [Required]
        public string Content { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        public bool IsReady { get; set; }

        public string Slug { get; set; }
        
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
