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
using TimeSlotting.Models.Deliveries;
using System.Collections.Generic;

namespace TimeSlotting.Controllers
{
    public class VendorsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        /// <summary>
        /// doesnt display deleted
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<VendorListEntryViewModel>))]
        public IHttpActionResult GetVendors()
        {
            return Ok(db.Vendors.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new VendorListEntryViewModel(el)).ToList());
        }

        /// <summary>
        /// displays only normal (no deleted and disabled)
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<VendorListEntryViewModel>))]
        public IHttpActionResult GetVendorList()
        {
            return Ok(db.Vendors.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList().Select(el => new VendorListEntryViewModel(el)).ToList());
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

            return Ok(new VendorListEntryViewModel(vendor));
        }

        /// <summary>
        /// Creation and modification of the vendor
        /// </summary>
        /// <param name="model">Set model id to 0 if creating new. When creating/modfying only Name and EntityStatus is take into account, rest is auto completed.</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutVendor(VendorListEntryViewModel model)
        {
            Vendor vendor = new Vendor();

            if (model.Id == 0)
            {
                vendor.Name = model.Name;
                vendor.EntityStatus = model.EntityStatus;
                vendor.CreationDate = DateTime.UtcNow;
                vendor.ModificationDate = DateTime.UtcNow;
                vendor.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                vendor.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.Vendors.Add(vendor);
            }
            else
            {
                vendor = db.Vendors.Find(model.Id);

                vendor.Name = model.Name;
                vendor.EntityStatus = model.EntityStatus;
                vendor.ModificationDate = DateTime.UtcNow;
                vendor.ModifiedById = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();

            vendor = db.Vendors.Include(e => e.CreatedBy).Include(e => e.ModifiedBy).SingleOrDefault(c => c.Id == vendor.Id);

            return Ok(new VendorListEntryViewModel(vendor));
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
                vendor.EntityStatus = EntityStatus.DELETED;
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