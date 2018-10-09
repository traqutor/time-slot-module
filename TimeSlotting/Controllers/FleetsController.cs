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
            var fleets = db.Fleets.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
            if (!User.IsInRole("Administrator"))
            {
                id = Common.GetCustomerId(User.Identity.GetUserId());
                fleets = fleets.Where(x => x.CustomerId == id).ToList();
            }

            return Ok(new { data = fleets, admin = User.IsInRole("Administrator"), cid = id });
        }

        public IHttpActionResult GetFleets(int id)
        {
            return Ok(db.Fleets.Where(x => x.CustomerId ==  id && !x.IsDeleted).OrderBy(x => x.Name).ToList());
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
                    fleet.IsDeleted = false;
                    fleet.CreatedDate = DateTime.Now;
                    fleet.ModifiedDate = DateTime.Now;
                    fleet.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Fleets.Add(fleet);
                }
                else
                {
                    fleet.ModifiedDate = DateTime.Now;
                    fleet.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(fleet).State = EntityState.Modified;
                    db.Entry(fleet).Property(x => x.CreatedDate).IsModified = false;
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
                fleet.IsDeleted = true;
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