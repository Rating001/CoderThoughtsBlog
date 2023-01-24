using CoderThoughtsBlog.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CoderThoughtsBlog.Enums;
using CoderThoughtsBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoderThoughtsBlog.Services
{
    public class UserDataService
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserDataService(UserManager<BlogUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        //public async static Task Initialize(IServiceProvider serviceProvider)
        //{
        //    object serviceProvider = null;
        //    var context = serviceProvider.GetService<ApplicationDbContext>();
        //    //Use a list of strings or a list to input roles into the database.
        //    //string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };
        //    List<BlogRole> rolesList = new List<BlogRole>();

        public async Task Initialize()
        {
                List<BlogRole> rolesList = new List<BlogRole>();

                foreach (var role in Enum.GetNames(typeof(BlogRole)))
                {
                    var roleStore = new RoleStore<IdentityRole>(_context);
                    //If there are no roles in the database, create the roles in BlogRole
                    if (!_context.Roles.Any())
                    {
                        roleStore.CreateAsync(new IdentityRole(role));
                    }
                }

                //If there are no users in the database, create the admin and mod user.
                if (_context.Users.Any())
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

                    //2) Use the Password Hasher, then create and store a new user that is defined by the adminUser
                    if (!_context.Users.Any(u => u.UserName == adminUser.UserName))
                    {
                        var password = new PasswordHasher<BlogUser>();
                        var hashed = password.HashPassword(adminUser, "secret");
                        adminUser.PasswordHash = hashed;

                        var userStore = new UserStore<BlogUser>(_context);
                        var result = userStore.CreateAsync(adminUser);

                    }

                    //3) Add this new user to the Administrator Role
                    await AssignRoles(adminUser.Email, Enum.GetNames(typeof(BlogRole)));


                    //Save the database changes
                    await _context.SaveChangesAsync();
                }

        }
            public async Task<IdentityResult> AssignRoles(string email, string[] roles)
            {
                BlogUser user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.AddToRolesAsync(user, roles);

                return result;
            }

    }
}