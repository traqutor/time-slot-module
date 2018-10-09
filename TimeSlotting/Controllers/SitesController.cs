using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using TimeSlotting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace TimeSlotting.Controllers
{
    [HandleError]
    public class SitesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult GetSites()
        {
            int? id = null;
            var sites = db.Sites.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                sites = sites.Where(x => x.CustomerId == id).ToList();
            }

            return Ok(new { data = sites, admin = User.IsInRole("Administrator"), cid = id });
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetSites(int id)
        {
            return Ok(db.Sites.Where(x => x.CustomerId == id && !x.IsDeleted).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult GetSite(int id)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return NotFound();
            }

            return Ok(site);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult PutSite(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Site site = (Site)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Site));

                if (site.Id == 0)
                {
                    site.IsDeleted = false;
                    site.CreatedDate = DateTime.Now;
                    site.ModifiedDate = DateTime.Now;
                    site.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    site.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Sites.Add(site);
                }
                else
                {
                    site.ModifiedDate = DateTime.Now;
                    site.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(site).State = EntityState.Modified;
                    db.Entry(site).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(site).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Site Data";
            }

            return Ok(response);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult DeleteSite(int id)
        {
            var response = "OK";

            Site site = db.Sites.Find(id);
            if (site == null)
            {
                response = "Site Not Found";
            }
            else
            {
                site.IsDeleted = true;
                db.SaveChanges();
            }

            return Ok(response);
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