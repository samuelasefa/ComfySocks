using System;
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
            CreateUserandRoles();
            //app.MapSignalR();
        }

        private void CreateUserandRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Super Admin")) {
                var role = new IdentityRole();
                role.Name = "Super Admin";
                roleManager.Create(role);

                //create super admin user
                var user = new ApplicationUser {
                    UserName = "SystemAdmin",
                    Email = "Info@ComfySocks.com",
                    FullName = "Comfy Socks Manufacturing PLC", IsActive = true
                };
                var result = userManager.Create(user, "Incorrect@44");

                userManager.AddToRole(user.Id, "Super Admin");
                
            }
            if (!roleManager.RoleExists("Admin")) {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Store Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Store Manager";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Sales"))
            {
                var role = new IdentityRole();
                role.Name = "Sales";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Finance"))
            {
                var role = new IdentityRole();
                role.Name = "Finance";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Production"))
            {
                var role = new IdentityRole();
                role.Name = "Production";
                roleManager.Create(role);
            }
        }
    }
}
