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
using TimeSlotting.Data;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Data.Entities;

namespace TimeSlotting.Controllers
{
    public class VendorsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVendors()
        {
            return Ok(db.Vendors.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVendorList()
        {
            return Ok(db.Vendors.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList());
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
                    vendor.EntityStatus = EntityStatus.NORMAL;
                    vendor.CreationDate = DateTime.UtcNow;
                    vendor.ModificationDate = DateTime.UtcNow;
                    vendor.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    vendor.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Vendors.Add(vendor);
                }
                else
                {
                    vendor.ModificationDate = DateTime.UtcNow;
                    vendor.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(vendor).State = EntityState.Modified;
                    db.Entry(vendor).Property(x => x.CreationDate).IsModified = false;
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
                vendor.EntityStatus = EntityStatus.NORMAL;
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