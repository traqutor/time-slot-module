using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using TimeSlotting.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities.Customers;
using TimeSlotting.Data.Entities;
using System.Web.Http.Description;
using TimeSlotting.Models.Customers;
using System.Collections.Generic;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class CustomersController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [ResponseType(typeof(List<CustomerListEntryViewModel>))]
        public IHttpActionResult GetCustomers()
        {
            return Ok(db.Customers.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(c => new CustomerListEntryViewModel(c)).ToList());
        }

        [ResponseType(typeof(CustomerListEntryViewModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(new CustomerListEntryViewModel(customer));
        }

        /// <summary>
        /// Creation and modification of the customer
        /// </summary>
        /// <param name="model">Set model id to 0 if creating new. When creating/modfying only Name and EntityStatus is take into account, rest is auto completed.</param>
        /// <returns></returns>
        [ResponseType(typeof(CustomerListEntryViewModel))]
        public IHttpActionResult PutCustomer(CustomerListEntryViewModel model)
        {
            Customer customer = new Customer();
            if (model.Id == 0)
            {
                customer.Name = model.Name;
                customer.EntityStatus = model.EntityStatus;
                customer.CreationDate = DateTime.UtcNow;
                customer.ModificationDate = DateTime.UtcNow;
                customer.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                customer.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.Customers.Add(customer);
            }
            else
            {
                customer = db.Customers.Find(model.Id);
                customer.EntityStatus = model.EntityStatus;
                customer.ModificationDate = DateTime.UtcNow;
                customer.ModifiedById = Common.GetUserId(User.Identity.GetUserId());
                customer.Name = model.Name;
            }

            db.SaveChanges();

            customer = db.Customers.Include(e => e.CreatedBy).Include(e => e.ModifiedBy).SingleOrDefault(c => c.Id == customer.Id);

            return Ok(new CustomerListEntryViewModel(customer));
        }

        public IHttpActionResult DeleteCustomer(int id)
        {
            var response = "OK";

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                response = "Customer Not Found";
            }
            else
            {
                customer.EntityStatus = EntityStatus.DELETED;
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