//using CoderThoughtsBlog.Data;
//using CoderThoughtsBlog.Services.Interfaces;
//using CoderThoughtsBlog.Services;
//using CoderThoughtsBlog.ViewModels;
//using CoderThoughtsBlog.Models;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration.UserSecrets;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
////var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
////var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];
//var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<BlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

//// Custom Services

////Register a Pre-Configured instance of the MailSettings class
//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//builder.Services.AddScoped<IBlogEmailSender, EmailService>();

////Register the Image Service
//builder.Services.AddScoped<IImageService, BasicImageService>();

////Register the Slug Service
//builder.Services.AddScoped<ISlugService, SlugService>();

////Register the Search Service
//builder.Services.AddScoped<BlogSearchService>();

//var app = builder.Build();

//var scope = app.Services.CreateScope();
////Updates the database with the latest migrations
//await DataService.ManageDataAsync(scope.ServiceProvider);


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");



//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.Run();








////using CoderThoughtsBlog.Controllers;
////using CoderThoughtsBlog.Data;
////using CoderThoughtsBlog.Models;
////using CoderThoughtsBlog.Services;
////using CoderThoughtsBlog.Services.Interfaces;
////using CoderThoughtsBlog.ViewModels;
////using ContactPro.Helpers;
////using Microsoft.AspNetCore.Builder;
////using Microsoft.AspNetCore.Hosting;
////using Microsoft.AspNetCore.HttpsPolicy;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity.UI;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.EntityFrameworkCore.Design;
////using Microsoft.Extensions.Configuration;
////using Microsoft.Extensions.DependencyInjection;
////using Microsoft.Extensions.Hosting;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using CoderThoughtsBlog.Services

////namespace CoderThoughtsBlog
////{
////    public class Startup
////    {
////        public Startup(IConfiguration configuration)
////        {
////            Configuration = configuration;
////        }

////        public IConfiguration Configuration { get; }


////        // This method gets called by the runtime. Use this method to add services to the container.
////        public void ConfigureServices(IServiceCollection services)
////        {
////            var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);
////            services.AddDbContext<ApplicationDbContext>(options =>
////                options.UseNpgsql(
////                    Configuration.GetConnectionString("DefaultConnection")));

////            services.AddDatabaseDeveloperPageExceptionFilter();


////            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
////            //    .AddEntityFrameworkStores<ApplicationDbContext>();

////            services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
////                .AddDefaultUI()
////                .AddDefaultTokenProviders()
////                .AddEntityFrameworkStores<ApplicationDbContext>();
////            services.AddControllersWithViews();
////            services.AddRazorPages();

////            //Register the custom DataService Class
////            services.AddScoped<DataService>();

////            //Register a Pre-Configured instance of the MailSettings class
////            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
////            services.AddScoped<IBlogEmailSender, EmailService>();

////            //Register the Image Service
////            services.AddScoped<IImageService, BasicImageService>();

////            //Register the Slug Service
////            services.AddScoped<ISlugService, SlugService>();

////            //Register the Search Service
////            services.AddScoped<BlogSearchService>();
////        }

////        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
////        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
////        {
////            if (env.IsDevelopment())
////            {
////                app.UseDeveloperExceptionPage();
////                app.UseMigrationsEndPoint();
////            }
////            else
////            {
////                app.UseExceptionHandler("/Home/Error");
////                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
////                app.UseHsts();
////            }
////            app.UseHttpsRedirection();
////            app.UseStaticFiles();

////            app.UseRouting();

////            app.UseAuthentication();
////            app.UseAuthorization();

////            app.UseEndpoints(endpoints =>
////            {
////                endpoints.MapControllerRoute(
////                    name: "SlugRoute",
////                    pattern: "BlogPosts/UrlFriendly/{slug}",
////                    defaults: new { controller = "Posts", action = "Details" });

////                endpoints.MapControllerRoute(
////                    name: "default",
////                    pattern: "{controller=Home}/{action=Index}/{id?}");



////                endpoints.MapRazorPages();
////            });
////        }
////    }
////}
