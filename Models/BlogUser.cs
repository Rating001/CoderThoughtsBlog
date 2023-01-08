using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderThoughtsBlog.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [StringLength(30,ErrorMessage="The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
        //public IFormFile ImageFile { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string? FacebookUrl { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string? TwitterUrl { get; set; }
        [StringLength(3000, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string? UserSignature { get; set; }

        [NotMapped]
        public string FullName 
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        //Navigation/Collection Properties

        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();


    }
}
