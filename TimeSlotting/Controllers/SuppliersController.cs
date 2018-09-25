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
    public class SuppliersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult GetSuppliers()
        {
            return Ok(db.Suppliers.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetSupplierList()
        {
            return Ok(db.Suppliers.Where(x => !x.IsDeleted && x.IsEnabled).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult GetSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutSupplier(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Supplier supplier = (Supplier)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Supplier));

                if (supplier.Id == 0)
                {
                    supplier.IsDeleted = false;
                    supplier.CreatedDate = DateTime.Now;
                    supplier.ModifiedDate = DateTime.Now;
                    supplier.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    supplier.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Suppliers.Add(supplier);
                }
                else
                {
                    supplier.ModifiedDate = DateTime.Now;
                    supplier.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(supplier).State = EntityState.Modified;
                    db.Entry(supplier).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(supplier).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Supplier Data";
            }

            return Ok(response);
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
                supplier.IsDeleted = true;
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