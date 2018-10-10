using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web.Http;
using TimeSlotting.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Owin.Security.DataProtection;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities.Customers.Fleets;
using TimeSlotting.Models.Users;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeSlotting.Controllers
{
    [HandleError]
    public class WebUsersController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public WebUsersController()
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetRoleName(string id)
        {
            return Ok(Common.GetRoleName(id));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetUserName(int id)
        {
            return Ok(Common.GetUserName(id));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult GetUsers()
        {
            var userList = new List<Tuple<WebUser, User, String>>();
            var users = (from m in db.WebUsers
                         join u in db.Users on m.ASPId equals u.Id
                         where m.EntityStatus != EntityStatus.DELETED
                         select new { m, u }).OrderBy(m => m.m.LastName).ToList();

            int? id = null;
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                users = users.Where(x => x.m.CustomerId == id).ToList();
            }

            foreach (dynamic user in users)
            {
                var role = Common.GetRoleName(((User)user.u).Roles.First().RoleId);
                Tuple<WebUser, User, String> tuple = new Tuple<WebUser, User, String>(user.m, user.u, role);
                userList.Add(tuple);
            }

            return Ok(new { data = userList, admin = User.IsInRole("Administrator"), cid = id });
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]

        public IHttpActionResult GetUser(int id)
        {
            WebUser user = db.WebUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var aspUser = db.Users.Find(user.ASPId);

            List<int> vehicles = new List<int>();
            var list = db.VehicleDrivers.Where(x => x.WebUserId == id).ToList();            
            foreach (VehicleDriver item in list)
            {
                vehicles.Add(item.VehicleId);
            }

            var result = new { user = user, email = Common.GetUserEmail(id), role = Common.GetRoleName(aspUser.Roles.First().RoleId), vehicles = vehicles };

            return Ok(result);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetUserRole()
        {
            var aspUser = db.Users.Find(User.Identity.GetUserId());
            var role = Common.GetRoleName(aspUser.Roles.First().RoleId);
            var cid = Common.GetUser(User.Identity.GetUserId()).CustomerId;

            return Ok(new { role = role, cid = cid, uid = Common.GetUserId(User.Identity.GetUserId()) });
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetDrivers(int cid)
        {
            var users = (from m in db.WebUsers
                         join u in db.Users on m.ASPId equals u.Id
                         where m.EntityStatus != EntityStatus.DELETED && m.CustomerId == cid
                         select new { m, u }).OrderBy(x => x.m.LastName).ToList();

            List<WebUser> drivers = new List<WebUser>();
            foreach (dynamic item in users)
            {
                if (item.u.Roles[0].RoleId == "3")
                {
                    drivers.Add(item.m);
                }
            }

            return Ok(drivers);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult PutUser(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                WebUser user = (WebUser)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(WebUser));

                var email = jsonResult.First.Next.First.ToString();
                var password = jsonResult.First.Next.Next.First.ToString();
                var role = jsonResult.First.Next.Next.Next.First.ToString();
                var vehicles = jsonResult.First.Next.Next.Next.Next.First.ToString();

                var userManager = Common.GetUserManager();
                if (user.Id == 0)
                {                    
                    var applicationUser = new User();
                    applicationUser.Email = email;
                    applicationUser.UserName = email;

                    var result = userManager.Create(applicationUser, password);
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(applicationUser.Id, role);

                        user.ASPId = applicationUser.Id;
                        user.EntityStatus = EntityStatus.NORMAL;
                        user.CreationDate = DateTime.UtcNow;
                        user.ModificationDate = DateTime.UtcNow;
                        user.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                        user.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                        db.WebUsers.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            response = error;
                        }
                    }
                }
                else
                {
                    var userASP = userManager.FindById(user.ASPId);
                    userASP.Email = email;
                    userASP.UserName = email;

                    userManager.RemoveFromRole(user.ASPId, userManager.GetRoles(user.ASPId)[0]);
                    userManager.AddToRole(user.ASPId, role);

                    var result = userManager.UpdateAsync(userASP);
                    foreach (var error in result.Result.Errors)
                    {
                        response = error;
                    }

                    if (result.Result.Succeeded)
                    {
                        user.ModificationDate = DateTime.UtcNow;
                        user.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                        db.Entry(user).State = EntityState.Modified;
                        db.Entry(user).Property(x => x.ASPId).IsModified = false;
                        db.Entry(user).Property(x => x.CreationDate).IsModified = false;
                        db.Entry(user).Property(x => x.CreatedBy).IsModified = false;
                        db.SaveChanges();

                        if (password != "")
                        {
                            var provider = new DpapiDataProtectionProvider("Sample");
                            userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("PasswordReset"));

                            var token = userManager.GeneratePasswordResetToken(userASP.Id);
                            var reset = userManager.ResetPassword(userASP.Id, token, password);
                            foreach (var error in reset.Errors)
                            {
                                response = error;
                            }
                        }
                    }
                }

                if (vehicles != "" && response == "OK")
                {
                    db.VehicleDrivers.RemoveRange(db.VehicleDrivers.Where(x => x.WebUserId == user.Id));
                    db.SaveChanges();

                    var regos = vehicles.Split(',');
                    foreach (string rego in regos)
                    {
                        var id = Regex.Replace(rego, "[^0-9]", "");
                        if (id != "")
                        {
                            var item = new VehicleDriver();
                            item.WebUserId = user.Id;
                            item.VehicleId = Int32.Parse(id);

                            db.VehicleDrivers.Add(item);
                        }
                    }

                    db.SaveChanges();
                }
            }
            else
            {
                response = "No User Data";
            }

            return Ok(response);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult DeleteUser(int id)
        {
            var response = "OK";

            WebUser user = db.WebUsers.Find(id);
            if (user == null)
            {
                response = "User Not Found";
            }
            else
            {
                user.EntityStatus = EntityStatus.DELETED;
                db.SaveChanges();
            }

            return Ok(response);
        }


        public UserListEntryViewModel GetUserInfo()
        {
            string loggedUserId = User.Identity.GetUserId();
            var userToReturn = db.WebUsers.Include(u => u.Customer).SingleOrDefault(u => u.ASPId == loggedUserId);
            var possibleRoles = _roleManager.Roles.ToList();
            return new UserListEntryViewModel(userToReturn, possibleRoles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}