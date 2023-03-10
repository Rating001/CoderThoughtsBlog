using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace CoderThoughtsBlog.Services
{
    public class TagSearchService
    {
        private readonly ApplicationDbContext _context;

        public TagSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Search(string tag)
        {
            var tags = _context.Tags
                               .Where(t=>t.Text == tag)
                               .Include(p=>p.Post)
                               .AsQueryable();



                                //.Include(p => p.Tags)
                                //.Where(p => p.ReadyStatus == Enums.ReadyStatus.ProductionReady)
                                //.AsQueryable();

            //var posts = _context.Posts.Include(p => p.Tags)
            //                         .ThenInclude(t => t.Text == tag && t.Post.ReadyStatus == Enums.ReadyStatus.ProductionReady)
            //                         .AsQueryable();

            //var searchResults = posts.Concat(posts);

            if (tag is not null)
            {
                tag = tag.ToLower();

                tags = tags.Where(t => t.Text == tag);

            //p.Title.ToLower().Contains(searchTerm) ||
            //p.Abstract.ToLower().Contains(searchTerm) ||
            //p.Content.ToLower().Contains(searchTerm) ||
            //p.Comments.Any(c => c.Body.ToLower().Contains(searchTerm) ||
            //                    c.ModeratedBody.ToLower().Contains(searchTerm) ||
            //                    c.BlogUser.FirstName.ToLower().Contains(searchTerm) ||
            //                    c.BlogUser.LastName.ToLower().Contains(searchTerm) ||
            //                    c.BlogUser.Email.ToLower().Contains(searchTerm)));



                }

            return tags;

        }


    }
}
