using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using TimeSlotting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Customers;
using TimeSlotting.Models.Customers;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace TimeSlotting.Controllers
{
    [HandleError]
    public class SitesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        /// <summary>
        /// gets site with customer id based on the logged user
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(List<SiteListEntryViewModel>))]
        public IHttpActionResult GetSites()
        {
            int? id = null;
            var sites = db.Sites.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).AsQueryable();

            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                sites = sites.Where(x => x.CustomerId == id);
            }

            return Ok(sites.ToList().Select(el => new SiteListEntryViewModel(el)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<SiteListEntryViewModel>))]
        public IHttpActionResult GetSites(int id)
        {
            return Ok(db.Sites.Where(x => x.CustomerId == id && x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new SiteListEntryViewModel(el)));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(SiteListEntryViewModel))]
        public IHttpActionResult GetSite(int id)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return NotFound();
            }

            return Ok(new SiteListEntryViewModel(site));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(SiteListEntryViewModel))]
        public IHttpActionResult PutSite(SiteListEntryViewModel model)
        {
            Site site = new Site();

            if (model.Id == 0)
            {
                site.Name = model.Name;
                site.CustomerId = model.Customer.Id;
                site.EntityStatus = model.EntityStatus;

                site.CreationDate = DateTime.UtcNow;
                site.ModificationDate = DateTime.UtcNow;
                site.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                site.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Sites.Add(site);
            }
            else
            {
                site = db.Sites.Find(model.Id);

                site.Name = model.Name;
                site.CustomerId = model.Customer.Id;
                site.EntityStatus = model.EntityStatus;

                site.ModificationDate = DateTime.UtcNow;
                site.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();
          
            return Ok(new SiteListEntryViewModel(site));
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
                site.EntityStatus = EntityStatus.DELETED;
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