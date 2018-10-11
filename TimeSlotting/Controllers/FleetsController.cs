using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using TimeSlotting.Models;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Customers.Fleets;
using System.Collections.Generic;
using TimeSlotting.Models.Customers.Fleets;
using System.Web.Http.Description;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
    [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
    public class FleetsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();
        /// <summary>
        /// gets fleets with customer id based on the logged user
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<FleetListEntryViewModel>))]
        public IHttpActionResult GetFleets()
        {
            int? id = null;
            var fleets = db.Fleets.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).AsQueryable();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                fleets = fleets.Where(x => x.CustomerId == id);
            }

            return Ok(fleets.ToList().Select(el => new FleetListEntryViewModel(el)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        [ResponseType(typeof(List<FleetListEntryViewModel>))]
        public IHttpActionResult GetFleets(int id)
        {
            return Ok(db.Fleets.Where(x => x.CustomerId ==  id && x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new FleetListEntryViewModel(el)));
        }

        [ResponseType(typeof(FleetListEntryViewModel))]
        public IHttpActionResult GetFleet(int id)
        {
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return NotFound();
            }

            return Ok(new FleetListEntryViewModel(fleet));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"> provide Name, Customer.Id</param>
        /// <returns></returns>
        [ResponseType(typeof(FleetListEntryViewModel))]
        public IHttpActionResult PutFleet(FleetListEntryViewModel model)
        {
           
            Fleet fleet = new Fleet();

            if (model.Id == 0)
            {
                fleet.Name = model.Name;
                fleet.CustomerId = model.Customer.Id;
                fleet.EntityStatus = model.EntityStatus;

                fleet.CreationDate = DateTime.UtcNow;
                fleet.ModificationDate = DateTime.UtcNow;
                fleet.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Fleets.Add(fleet);
            }
            else
            {
                fleet.Name = model.Name;
                fleet.CustomerId = model.Customer.Id;
                fleet.EntityStatus = model.EntityStatus;

                fleet.ModificationDate = DateTime.UtcNow;
                fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();
         
            return Ok(fleet);
        }

        public IHttpActionResult DeleteFleet(int id)
        {
            var response = "OK";

            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                response = "Fleet Not Found";
            }
            else
            {
                fleet.EntityStatus = EntityStatus.DELETED;
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