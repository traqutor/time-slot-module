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

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class CustomersController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        public IHttpActionResult GetCustomers()
        {
            return Ok(db.Customers.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList());
        }

        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public IHttpActionResult PutCustomer(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Customer customer = (Customer)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Customer));

                if (customer.Id == 0)
                {
                    customer.EntityStatus = EntityStatus.NORMAL;
                    customer.CreationDate = DateTime.UtcNow;
                    customer.ModificationDate = DateTime.UtcNow;
                    customer.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    customer.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Customers.Add(customer);
                }
                else
                {
                    customer.ModificationDate = DateTime.UtcNow;
                    customer.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(customer).State = EntityState.Modified;
                    db.Entry(customer).Property(x => x.CreationDate).IsModified = false;
                    db.Entry(customer).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Customer Data";
            }

            return Ok(response);
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
                customer.EntityStatus = EntityStatus.NORMAL;
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