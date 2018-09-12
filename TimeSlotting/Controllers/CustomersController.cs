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

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetCustomers()
        {
            return Ok(db.Customers.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList());
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
                    customer.IsDeleted = false;
                    customer.CreatedDate = DateTime.Now;
                    customer.ModifiedDate = DateTime.Now;
                    customer.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    customer.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Customers.Add(customer);
                }
                else
                {
                    customer.ModifiedDate = DateTime.Now;
                    customer.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(customer).State = EntityState.Modified;
                    db.Entry(customer).Property(x => x.CreatedDate).IsModified = false;
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

            customer.IsDeleted = true;
            db.SaveChanges();

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