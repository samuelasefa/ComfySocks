﻿using System;
using ComfySocks.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComfySocks.Startup))]
namespace ComfySocks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createUserandRoles();
        }

        private void createUserandRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Super Admin")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Super Admin";
                roleManager.Create(role);

                //create super admin user
                var user = new ApplicationUser {
                    UserName = "ComfySocks",
                Email = "Info@ComfySocks.com",
                  FullName = "Comfy Socks Manufacturing PLC", IsActive = true
                };
                var result = userManager.Create(user, "!1234Aa");

                userManager.AddToRole(user.Id, "Super Admin");
                
            }
            if (!roleManager.RoleExists("Admin")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Store Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Store Manager";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Sales"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Sales";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Finance"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Finance";
                roleManager.Create(role);
            }
        }
    }
}