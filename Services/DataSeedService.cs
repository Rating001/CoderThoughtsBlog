using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Enums;
using CoderThoughtsBlog.Models;
using CoderThoughtsBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace CoderThoughtsBlog.Services
{

    public class DataSeedService : IDataSeedService
    {
        private readonly ApplicationDbContext? _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataSeedService(ApplicationDbContext dbContext,
                           RoleManager<IdentityRole> roleManager,
                           UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //public async Task SeedAsync()
        //{
        //    //Seed a few roles into the system
        //    await SeedRolesAsync();

        //    //Seed a few users into the system
        //    await SeedUsersAsync();
        //}
        public async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any())
            {
            }
            else
            {
                //Otherwise, we want to create a few Roles
                foreach (var role in Enum.GetNames(typeof(BlogRole)))
                {
                    //Use the Role Manager to create Roles
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public async Task SeedUsersAsync()
            {
            //If there are already Users in the system, do nothing
            if (_dbContext.Users.Any())
            {
            }
            else
            {
                //1) Create a new instance of BlogUser
                
                var adminUser = new BlogUser()
                {
                    Email = "rating001@aol.com",
                    UserName = "rating001@aol.com",
                    DisplayName = "Ken Codes",
                    FirstName = "Ken",
                    LastName = "Klein",
                    PhoneNumber = "5099392153",
                    EmailConfirmed = true
                };

                //2) Use the UserManager to create a new user that is defined by the adminUser
                await _userManager.CreateAsync(adminUser, "Washington1!");

                //3) Add this new user to the Administrator Role
                await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


                //1) Create a new instance of BlogUser
                var modUser = new BlogUser()
                {
                    Email = "kklein@crucialcleaners.com",
                    UserName = "kklein@crucialcleaners.com",
                    DisplayName = "KenCodes2",
                    FirstName = "Ken",
                    LastName = "Klein",
                    PhoneNumber = "5099392153",
                    EmailConfirmed = true
                };

                //2) Use the UserManager to create a new user that is defined by the modUser
                await _userManager.CreateAsync(modUser, "Washington1!");

                //3) Add this new user to the Moderator Role
                await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());

                await _dbContext.SaveChangesAsync();
            }
        }
            
        }
    }




