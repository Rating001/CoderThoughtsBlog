using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Services.Interfaces;
using CoderThoughtsBlog.Services;
using CoderThoughtsBlog.ViewModels;
using CoderThoughtsBlog.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];
var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<BlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Custom Services

//Register a Pre-Configured instance of the MailSettings class
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IBlogEmailSender, EmailService>();

//Register the Image Service
builder.Services.AddScoped<IImageService, BasicImageService>();

//Register the Slug Service
builder.Services.AddScoped<ISlugService, SlugService>();

builder.Services.AddScoped<IDataSeedService, DataSeedService>();

//Register the Search Service
builder.Services.AddScoped<BlogSearchService>();

//Register the Search Service
builder.Services.AddScoped<TagSearchService>();

////Register the UserDataService
builder.Services.AddScoped<UserDataService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
//Updates the database with the latest migrations
await DataService.ManageDataAsync(scope.ServiceProvider);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();






//using CoderThoughtsBlog.Services;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CoderThoughtsBlog
//{
//    public class Program
//    {
//        public static async Task Main(string[] args)
//        {
//            //CreateHostBuilder(args).Build().Run();
//            var host = CreateHostBuilder(args).Build();

//            //Pull out the registered DataService
//            var dataService = host.Services
//                                  .CreateScope()
//                                  .ServiceProvider
//                                  .GetRequiredService<DataService>();


//            host.Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}
