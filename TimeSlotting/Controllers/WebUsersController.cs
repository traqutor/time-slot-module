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
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;

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

        /// <summary>
        /// returns UsersList
        /// includes Customer,Site,Fleet
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(List<UserListEntryViewModel>))]
        public IHttpActionResult GetUsers()
        {
            /* Old code
             * var userList = new List<Tuple<WebUser, User, String>>();
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
            */

            var users = db.WebUsers.Include(u => u.Customer).Include(u => u.Site).Include(u => u.Fleet).Where(u => u.EntityStatus != EntityStatus.DELETED).AsQueryable();
                        
            int? id = null;
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                users = users.Where(x => x.CustomerId == id);
            }

            var usersList = users.AsNoTracking().ToList();

            var roles = db.Roles.OrderByDescending(r => r.Id).ToList(); 

            return Ok(users.ToList().Select(el => new UserListEntryViewModel(el, roles)));
        }

        /// <summary>
        /// returns a user with a given id
        /// includes Customer,Site,Fleet,Vehicles
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(UserListEntryViewModel))]
        public IHttpActionResult GetUser(int id)
        {
            /* Old code
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
            */
            WebUser user = db.WebUsers.Include(u => u.Customer).Include(u => u.Site).Include(u => u.Fleet).Include("VehicleDrivers.Vehicle").AsNoTracking().SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = db.Roles.OrderByDescending(r => r.Id).ToList();
            
            return Ok(new UserListEntryViewModel(user, roles));
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

        /// <summary>
        /// get all drivers for a given customer
        /// </summary>
        /// <param name="cid">customer id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<UserListEntryViewModel>))]
        public IHttpActionResult GetDrivers(int cid)
        {
            /* old code
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
            */

            var roles = db.Roles.OrderByDescending(r => r.Id).ToList();
            string driverRoleId = roles.SingleOrDefault(r => r.Name == UserSystemRoles.Driver)?.Id;

            var users = db.WebUsers.Where(u => u.EntityStatus != EntityStatus.DELETED && u.CustomerId == cid && u.User.Roles.Any(r => r.RoleId == driverRoleId))
                .OrderBy(u => u.LastName).AsNoTracking();

            return Ok(users.ToList().Select(el => new UserListEntryViewModel(el, roles)));
        }

        /// <summary>
        /// updates or creates user
        /// </summary>
        /// <param name="model">email, password, Role.Name - required for new user creation
        /// optional: Customer.Id, Site.Id, Fleet.Id, Vehicles.Ids
        /// </param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(UserListEntryViewModel))]
        public IHttpActionResult PutUser(UserListEntryViewModel model)
        {
            WebUser user = new WebUser();

            var userManager = Common.GetUserManager();
            if (model.Id == 0)
            {                    
                var applicationUser = new User();
                applicationUser.Email = model.Email;
                applicationUser.UserName = model.Email;

                var result = userManager.Create(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(applicationUser.Id, model.Role.Name);

                    user.CustomerId = model.Customer?.Id;
                    user.FleetId = model.Fleet?.Id;
                    user.SiteId = model.Site?.Id;

                    user.ASPId = applicationUser.Id;
                    user.FirstName = model.Name;
                    user.LastName = model.Surname;

                    user.EntityStatus = EntityStatus.NORMAL;
                    user.CreationDate = DateTime.UtcNow;
                    user.ModificationDate = DateTime.UtcNow;
                    user.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                    user.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                    db.WebUsers.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(result.Errors));
                }
            }
            else
            {
                var aspUser = db.WebUsers.AsNoTracking().SingleOrDefault(wu => wu.Id == model.Id).User;

                aspUser.Email = model.Email;
                aspUser.UserName = model.Email;

                db.SaveChanges();

                userManager.RemoveFromRole(aspUser.Id, userManager.GetRoles(aspUser.Id)[0]);
                var result = userManager.AddToRole(aspUser.Id, model.Role.Name);

                //var result = userManager.UpdateAsync(aspUser);
             
                if (result.Succeeded)
                {
                    user = db.WebUsers.Find(model.Id);

                    user.FirstName = model.Name;
                    user.LastName = model.Surname;

                    user.CustomerId = model.Customer?.Id;
                    user.FleetId = model.Fleet?.Id;
                    user.SiteId = model.Site?.Id;

                    user.ModificationDate = DateTime.UtcNow;
                    user.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var provider = new DpapiDataProtectionProvider("Sample");
                        userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("PasswordReset"));

                        var token = userManager.GeneratePasswordResetToken(user.ASPId);
                        var reset = userManager.ResetPassword(user.ASPId, token, model.Password);

                        if(reset.Errors.Count() > 0)
                            return BadRequest(JsonConvert.SerializeObject(result.Errors));
                    }
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(result.Errors));
                }
            }

            if (model.Vehicles != null)
            {
                db.VehicleDrivers.RemoveRange(db.VehicleDrivers.Where(x => x.WebUserId == user.Id));
                db.SaveChanges();

                foreach (var vehicle in model.Vehicles)
                {
                    var id = vehicle.Id;
                    if (id != 0)
                    {
                        var item = new VehicleDriver();
                        item.WebUserId = user.Id;
                        item.VehicleId = id;

                        db.VehicleDrivers.Add(item);
                    }
                }
                db.SaveChanges();
            }

            var roles = db.Roles.OrderByDescending(r => r.Id).ToList();

            user = db.WebUsers.Include(c => c.User).Include(c => c.Fleet).Include(c => c.Customer).Include("VehicleDrivers.Vehicle").Include(c => c.Site).SingleOrDefault(u => u.Id == user.Id);

            return Ok(new UserListEntryViewModel(user, roles));
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
            var userToReturn = db.WebUsers.Include(u => u.Customer).AsNoTracking().SingleOrDefault(u => u.ASPId == loggedUserId);

            if (userToReturn == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

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