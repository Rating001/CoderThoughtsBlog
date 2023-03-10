using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderThoughtsBlog.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string? BlogUserId { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "Your blog {0} must be between {2} and {1} characters in length.", MinimumLength = 2)]
        public string? Name { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Your blog {0} must be between {2} and {1} characters in length.", MinimumLength = 2)]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Created")]
        public DateTime? Created { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Updated")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Blog Image")]
        public byte[]? ImageData { get; set; }
        [Display(Name = "Image Type")]
        public string? ContentType { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }

        //Navigation Properties
        [Display(Name="Author")]
        public virtual BlogUser? BlogUser { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();


    }
}
