using System.Collections.Generic;
using CoderThoughtsBlog.Models;

namespace CoderThoughtsBlog.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
