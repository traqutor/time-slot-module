using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TimeSlotting.Models;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Customers.Fleets;
using TimeSlotting.Models.Customers.Fleets;
using System.Collections.Generic;

namespace TimeSlotting.Controllers
{
    [HandleError]
    public class VehiclesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        /// <summary>
        /// gets vehicles from all fleets with customer id based on the logged user
        /// gets all vehicles for admin
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<VehicleListEntryViewModel>))]
        public IHttpActionResult GetVehicles()
        {
            int? id = null;
            var vehicles = db.Vehicles.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Rego).AsQueryable();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                vehicles = vehicles.Where(x => x.Fleet.CustomerId == id);
            }

            return Ok(vehicles.ToList().Select(el => new VehicleListEntryViewModel(el)));
        }

        /// <summary>
        /// get vehicles from specific fleet
        /// </summary>
        /// <param name="id">fleet id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<VehicleListEntryViewModel>))]
        public IHttpActionResult GetVehicles(int id)
        {
            return Ok(db.Vehicles.Where(x => x.FleetId == id && x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Rego).ToList().Select(el => new VehicleListEntryViewModel(el)));
        }

        /// <summary>
        /// gets vehicles for a specific driver
        /// </summary>
        /// <param name="uid">driver id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<VehicleListEntryViewModel>))]
        public IHttpActionResult GetDriverVehicles(int uid)
        {
            var vehicles = (from m in db.Vehicles
                         join d in db.VehicleDrivers on m.Id equals d.VehicleId
                         where m.EntityStatus == EntityStatus.NORMAL && d.WebUserId == uid
                         select m).OrderBy(m => m.Rego).ToList().Select(el => new VehicleListEntryViewModel(el));

            return Ok(vehicles);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(VehicleListEntryViewModel))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">when creating/edidit, provide Rego, Fleet.id, Fleet.Customer.Id</param>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        [ResponseType(typeof(VehicleListEntryViewModel))]
        public IHttpActionResult PutVehicle(VehicleListEntryViewModel model)
        {
            Vehicle vehicle = new Vehicle();

            if (model.Id == 0)
            {
                vehicle.Rego = model.Rego;
                vehicle.FleetId = model.Fleet.Id;
                vehicle.CustomerId = model.Fleet.Customer.Id;
                vehicle.EntityStatus = model.EntityStatus;

                vehicle.CreationDate = DateTime.UtcNow;
                vehicle.ModificationDate = DateTime.UtcNow;
                vehicle.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                vehicle.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Vehicles.Add(vehicle);
            }
            else
            {
                vehicle = db.Vehicles.Find(model.Id);

                vehicle.Rego = model.Rego;
                vehicle.FleetId = model.Fleet.Id;
                vehicle.CustomerId = model.Fleet.Customer.Id;
                vehicle.EntityStatus = model.EntityStatus;

                vehicle.ModificationDate = DateTime.UtcNow;
                vehicle.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Entry(vehicle).State = EntityState.Modified;
                db.Entry(vehicle).Property(x => x.CreationDate).IsModified = false;
                db.Entry(vehicle).Property(x => x.CreatedBy).IsModified = false;
            }

            db.SaveChanges();
          
            return Ok(vehicle);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult DeleteVehicle(int id)
        {
            var response = "OK";

            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                response = "Vehicle Not Found";
            }
            else
            {
                vehicle.EntityStatus = EntityStatus.DELETED;
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