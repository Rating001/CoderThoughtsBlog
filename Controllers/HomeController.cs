﻿using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Models;
using CoderThoughtsBlog.Services.Interfaces;
using CoderThoughtsBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoderThoughtsBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            //var blogs = await _context.Blogs
            //                          .Include(b => b.BlogUser)
            //                          .ToListAsync();

            var pageNumber = page ?? 1;
            var pageSize = 6;

            var blogs = _context.Blogs
                                .Include(u => u.BlogUser)
                                .Where(b => b.Posts.Any(p => p.ReadyStatus == Enums.ReadyStatus.ProductionReady))
                                .OrderByDescending(b => b.Updated)
                                .ToPagedListAsync(pageNumber, pageSize);

            return View(await blogs);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            //Send an email to the administrator
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
            await _emailSender.SendContactEmailAsync(model.Email,model.Name,model.Subject,model.Message);

            return(RedirectToAction("Index"));

            //Possible SWAL
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
