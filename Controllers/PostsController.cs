using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Models;
using CoderThoughtsBlog.Services;
using CoderThoughtsBlog.Services.Interfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace CoderThoughtsBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly BlogSearchService _blogSearchService;


        public PostsController(ApplicationDbContext context, ISlugService slugService, IImageService imageService, UserManager<BlogUser> userManager, BlogSearchService blogSearchService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
            _blogSearchService = blogSearchService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog).Include(p => p.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }


        [Authorize]
        // GET: Posts
        public async Task<IActionResult> BlogPostIndex(int? id, int? page)
        {
            if (id is null)
            {
                return NotFound();
            }

            var pageNumber = page ?? 1;
            var pageSize = 5;

            //var posts = _context.Posts.Where(p => p.BlogId == id);
            var posts = await _context.Posts
                                .Where(p => p.BlogId == id && p.ReadyStatus == Enums.ReadyStatus.ProductionReady)
                                .Include(p => p.BlogUser)
                                .OrderByDescending(p => p.Created)
                                .ToPagedListAsync(pageNumber,pageSize);

            return View(posts);
        }

        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 6;

            var posts = _blogSearchService.Search(searchTerm);

            return View(await posts.ToPagedListAsync(pageNumber, pageSize));
                         

        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.BlogUser)
                .FirstOrDefaultAsync(m => m.Slug == slug);



            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,ReadyStatus,Image")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                //Create the slug and determine if it is unique
                var slug = _slugService.UrlFriendly(post.Title);
                var newSlug = post.Slug; 
                //Create a variable to store whether an error has occurred
                var validationError = false;

                //post.Slug = slug;

                //Detect if incoming Slug is null
                if (string.IsNullOrEmpty(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("Title", "The Title cannot be used as it results in an empty slug.");
                }

                //Detect incoming duplicate Slugs
                else if (!_slugService.IsUnique(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("Title", "The title you provided cannot be used as it results in a duplicate slug.");
                }


                if(validationError)
                {
                    ViewData["TagValues"] = string.Join(",", tagValues);
                    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
                    return View(post);
                }

                post.Slug = slug;

                post.Created = DateTime.Now;
                //Set the current user to the BlogUserId (Security and necessity)
                var authorId = _userManager.GetUserId(User);
                post.BlogUserId = authorId;

                //Convert the image into a byte array.
                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ContentType = _imageService.ContentType(post.Image);

                _context.Add(post);
                await _context.SaveChangesAsync();

                //Loop over the incoming list of tagValues.
                foreach(var tagText in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId= post.Id,
                        BlogUserId = authorId,
                        Text = tagText
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Slug == slug);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,ReadyStatus")] Post post, IFormFile NewImage, List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var newPost = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == post.Id);

                    newPost.Updated = DateTime.Now;

                    if (post.Title is not null)
                    {
                        newPost.Title = post.Title;
                    }
                    if (post.Abstract is not null)
                    {
                        newPost.Abstract = post.Abstract;
                    }
                    if (post.Content is not null)
                    {
                        newPost.Content = post.Content;
                    }
                    if (newPost.ReadyStatus != post.ReadyStatus)
                    {
                        newPost.ReadyStatus = post.ReadyStatus;
                    }

                    var newSlug = _slugService.UrlFriendly(post.Title);

                    //Check is the original slug (newSlug) matches the new slug
                    if (newSlug!= newPost.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            newPost.Title = post.Title;
                            newPost.Slug = newSlug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "The title you provided cannot be used as it results in a duplicate slug.");
                            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
                            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
                            return View(post);
                        }
                    }

                    if (NewImage is not null)
                    {
                        newPost.ImageData = await _imageService.EncodeImageAsync(NewImage);
                        newPost.ContentType = NewImage.ContentType;
                    }

                    //Remove all previous Tags and Apply the new list (even if they are the same)
                    _context.Tags.RemoveRange(newPost.Tags);
                    foreach(var tagText in tagValues) 
                    {
                        _context.Add(new Tag()
                        {
                            PostId = post.Id,
                            BlogUserId = post.BlogUserId,
                            Text = tagText

                        });
                    }


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var blogId = post.BlogId.ToString();
                return RedirectToAction("BlogPostIndex", new { id = blogId });

            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);

            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .FirstOrDefaultAsync(m => m.Slug == slug);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string slug)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Slug == slug);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
