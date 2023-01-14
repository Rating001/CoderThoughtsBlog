﻿using CoderThoughtsBlog.Data;
using CoderThoughtsBlog.Enums;
using CoderThoughtsBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoderThoughtsBlog.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext,
                           RoleManager<IdentityRole> roleManager,
                           UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            //Create the database from the Migrations
            await _dbContext.Database.MigrateAsync();
            //Seed a few roles into the system
            await SeedRolesAsync();

            //Seed a few users into the system
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            //If there are already Roles in the system, do nothing
            if(_dbContext.Roles.Any())
            {
                return;
            }
            //Otherwise, we want to create a few Roles
            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                //Use the Role Manager to create Roles
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

        }

        private async Task SeedUsersAsync()
        {
            //If there are already Users in the system, do nothing
            if(_dbContext.Users.Any())
            {
                return;
            }

            //1) Create a new instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "rating001@aol.com",
                UserName = "rating001@aol.com",
                DisplayName = "KenCodes",
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
        }




        



        //
    }
}
