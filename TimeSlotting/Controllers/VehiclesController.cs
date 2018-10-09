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

namespace TimeSlotting.Controllers
{
    [HandleError]
    public class VehiclesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVehicles()
        {
            int? id = null;
            var vehicles = db.Vehicles.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Rego).ToList();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                vehicles = vehicles.Where(x => x.Fleet.CustomerId == id).ToList();
            }

            return Ok(new { data = vehicles, admin = User.IsInRole("Administrator"), cid = id });
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetVehicles(int id)
        {
            return Ok(db.Vehicles.Where(x => x.FleetId == id && x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Rego).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetDriverVehicles(int uid)
        {
            var vehicles = (from m in db.Vehicles
                         join d in db.VehicleDrivers on m.Id equals d.VehicleId
                         where m.EntityStatus == EntityStatus.NORMAL && d.WebUserId == uid
                         select m).OrderBy(m => m.Rego).ToList();

            return Ok(vehicles);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
        public IHttpActionResult PutVehicle(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Vehicle vehicle = (Vehicle)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Vehicle));

                if (vehicle.Id == 0)
                {
                    vehicle.EntityStatus = EntityStatus.NORMAL;
                    vehicle.CreationDate = DateTime.UtcNow;
                    vehicle.ModificationDate = DateTime.UtcNow;
                    vehicle.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    vehicle.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Vehicles.Add(vehicle);
                }
                else
                {
                    vehicle.ModificationDate = DateTime.UtcNow;
                    vehicle.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(vehicle).State = EntityState.Modified;
                    db.Entry(vehicle).Property(x => x.CreationDate).IsModified = false;
                    db.Entry(vehicle).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Vehicle Data";
            }

            return Ok(response);
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