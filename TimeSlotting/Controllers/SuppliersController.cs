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
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using System.Collections.Generic;
using TimeSlotting.Models.Deliveries;

namespace TimeSlotting.Controllers
{
    public class SuppliersController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        /// <summary>
        /// doesnt display deleted
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<SupplierListEntryViewModel>))]
        public IHttpActionResult GetSuppliers()
        {
            return Ok(db.Suppliers.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new SupplierListEntryViewModel(el)).ToList());
        }

        /// <summary>
        /// displays only normal (no deleted and disabled)
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<SupplierListEntryViewModel>))]
        public IHttpActionResult GetSupplierList()
        {
            return Ok(db.Suppliers.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList().Select(el => new SupplierListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(SupplierListEntryViewModel))]
        public IHttpActionResult GetSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(new SupplierListEntryViewModel(supplier));
        }

        /// <summary>
        /// Creation and modification of the supplier
        /// </summary>
        /// <param name="model">Set model id to 0 if creating new. When creating/modfying only Name and EntityStatus is take into account, rest is auto completed.</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutSupplier(SupplierListEntryViewModel model)
        {
            Supplier supplier = new Supplier();
         
            if (supplier.Id == 0)
            {
                supplier.Name = model.Name;
                supplier.EntityStatus = model.EntityStatus;
                supplier.CreationDate = DateTime.UtcNow;
                supplier.ModificationDate = DateTime.UtcNow;
                supplier.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                supplier.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.Suppliers.Add(supplier);
            }
            else
            {
                supplier = db.Suppliers.Find(model.Id);

                supplier.Name = model.Name;
                supplier.EntityStatus = model.EntityStatus;

                supplier.ModificationDate = DateTime.UtcNow;
                supplier.ModifiedById = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();

            supplier = db.Suppliers.Include(e => e.CreatedBy).Include(e => e.ModifiedBy).SingleOrDefault(c => c.Id == supplier.Id);

            return Ok(supplier);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteSupplier(int id)
        {
            var response = "OK";

            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                response = "Supplier Not Found";
            }
            else
            {
                supplier.EntityStatus = EntityStatus.DELETED;
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