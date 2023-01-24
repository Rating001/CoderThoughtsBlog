using CoderThoughtsBlog.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CoderThoughtsBlog.Enums;
using CoderThoughtsBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoderThoughtsBlog.Services
{
    public class SampleDataService
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            //Use a list of strings or a list to input roles into the database.
            //string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };
            List<BlogRole> rolesList = new List<BlogRole>();

            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                //If there are no roles in the database, create the roles in BlogRole
                if (!context.Roles.Any())
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            //If there are no users in the database, create the admin and mod user.
            if (context.Users.Any())
            {
                //1) Create a new instance of BlogUser
                var adminUser = new BlogUser()
                {
                    Email = "rating001@aol.com",
                    UserName = "rating001@aol.com",
                    DisplayName = "KenCodes",
                    FirstName = "Ken",
                    LastName = "Klein",
                    PhoneNumber = "5099392153",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                //2) Use the UserManager to create a new user that is defined by the adminUser
                if (!context.Users.Any(u => u.UserName == adminUser.UserName))
                {
                    var password = new PasswordHasher<BlogUser>();
                    var hashed = password.HashPassword(adminUser, "secret");
                    adminUser.PasswordHash = hashed;

                    var userStore = new UserStore<BlogUser>(context);
                    var result = userStore.CreateAsync(adminUser);

                }

                //3) Add this new user to the Administrator Role
                AssignRoles(serviceProvider, adminUser.Email, Enum.GetNames(typeof(BlogRole)));


                //Repeat steps 1 and 2 for any other required users.

                //1) Create a new instance of BlogUser



                if (!context.Users.Any(u => u.UserName == adminUser.UserName))
                {
                    var password = new PasswordHasher<BlogUser>();
                    var hashed = password.HashPassword(adminUser, "secret");
                    adminUser.PasswordHash = hashed;

                    var userStore = new UserStore<BlogUser>(context);
                    var result = userStore.CreateAsync(adminUser);

                }

                //2) Use the UserManager to create a new user that is defined by the modUser
                await _userManager.CreateAsync(modUser, "Washington1!");

                //3) Add this new user to the Moderator Role
                await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());





            }

            public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
            {
                UserManager<BlogUser> _userManager = services.GetService<UserManager<BlogUser>>();
                BlogUser user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.AddToRolesAsync(user, roles);

                return result;
            }

        }
    }
}