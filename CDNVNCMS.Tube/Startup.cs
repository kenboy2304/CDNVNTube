﻿using CDNVNCMS.Tube.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CDNVNCMS.Tube.Startup))]
namespace CDNVNCMS.Tube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }


        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesandUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser
                {
                    UserName = "admin@cdnvn.com",
                    Email = "admin@cdnvn.com"
                };

                var chkUser = userManager.Create(user, "CDNvn@123456");

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole
                {
                    Name = "Manager"
                };
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
                roleManager.Create(role);

            }
        }
    }
}
