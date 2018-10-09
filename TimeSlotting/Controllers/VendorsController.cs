using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using TimeSlotting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace TimeSlotting.Controllers
{
    public class VendorsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVendors()
        {
            return Ok(db.Vendors.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVendorList()
        {
            return Ok(db.Vendors.Where(x => !x.IsDeleted && x.IsEnabled).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(Vendor))]
        public IHttpActionResult GetVendor(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return Ok(vendor);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutVendor(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Vendor vendor = (Vendor)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Vendor));

                if (vendor.Id == 0)
                {
                    vendor.IsDeleted = false;
                    vendor.CreatedDate = DateTime.Now;
                    vendor.ModifiedDate = DateTime.Now;
                    vendor.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    vendor.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Vendors.Add(vendor);
                }
                else
                {
                    vendor.ModifiedDate = DateTime.Now;
                    vendor.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(vendor).State = EntityState.Modified;
                    db.Entry(vendor).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(vendor).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Vendor Data";
            }

            return Ok(response);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteVendor(int id)
        {
            var response = "OK";

            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                response = "Vendor Not Found";
            }
            else
            {
                vendor.IsDeleted = true;
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