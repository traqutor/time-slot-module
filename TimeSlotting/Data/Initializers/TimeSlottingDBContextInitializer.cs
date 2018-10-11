using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;

namespace TimeSlotting.Data.Initializers
{
    public class TimeSlottingDBContextInitializer : CreateDatabaseIfNotExists<TimeSlottingDBContext>
    {
        protected override void Seed(TimeSlottingDBContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<User>(new UserStore<User>(context));

            #region Roles creation

            //all roles defined in the UserSystemRoles class, go there if You want to add one
            foreach (var roleName in UserSystemRoles.PossibleRoles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    var role = new IdentityRole();
                    role.Name = roleName;
                    roleManager.Create(role);
                }
            }

            #region SystemAdmin creation

            var sysAdmin = new User();
            sysAdmin.UserName = "sysadmin@gmail.com";
            sysAdmin.Email = "sysadmin@gmail.com";
            sysAdmin.EmailConfirmed = true;

            //sysAdmin.CreationDate = DateTime.UtcNow;
            //sysAdmin.ModificationDate = DateTime.UtcNow;

            string userPWD = "Password";

            var chkUser = UserManager.Create(sysAdmin, userPWD);

            if (chkUser.Succeeded)
            {
                var result = UserManager.AddToRole(sysAdmin.Id, UserSystemRoles.Administrator);
            }

            WebUser webUser = new WebUser();
            webUser.FirstName = "System";
            webUser.LastName = "Admin";
            webUser.User = sysAdmin;
            webUser.CreationDate = DateTime.UtcNow;
            webUser.ModificationDate = DateTime.UtcNow;
      
            context.WebUsers.Add(webUser);
            context.SaveChanges();

            #endregion



            #endregion
        }
    }
}