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

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin")]
    [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin")]
    public class FleetsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        public IHttpActionResult GetFleets()
        {
            int? id = null;
            var fleets = db.Fleets.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                fleets = fleets.Where(x => x.CustomerId == id).ToList();
            }

            return Ok(new { data = fleets, admin = User.IsInRole("Administrator"), cid = id });
        }

        public IHttpActionResult GetFleets(int id)
        {
            return Ok(db.Fleets.Where(x => x.CustomerId ==  id && x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList());
        }

        public IHttpActionResult GetFleet(int id)
        {
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return NotFound();
            }

            return Ok(fleet);
        }

        public IHttpActionResult PutFleet(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Fleet fleet = (Fleet)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Fleet));

                if (fleet.Id == 0)
                {
                    fleet.EntityStatus = EntityStatus.NORMAL;
                    fleet.CreationDate = DateTime.UtcNow;
                    fleet.ModificationDate = DateTime.UtcNow;
                    fleet.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Fleets.Add(fleet);
                }
                else
                {
                    fleet.ModificationDate = DateTime.UtcNow;
                    fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(fleet).State = EntityState.Modified;
                    db.Entry(fleet).Property(x => x.CreationDate).IsModified = false;
                    db.Entry(fleet).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Fleet Data";
            }

            return Ok(response);
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
                fleet.EntityStatus = EntityStatus.NORMAL;
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