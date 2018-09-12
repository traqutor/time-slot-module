using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
    public class TimeSlotsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetTimeSlots()
        {
            return Ok(db.TimeSlots.Where(x => !x.IsDeleted).OrderBy(x => x.StartTime).ToList());
        }

        public IHttpActionResult GetTimeSlot(int id)
        {
            TimeSlot timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return Ok(timeslot);
        }

        public IHttpActionResult PutTimeSlot(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                TimeSlot timeslot = (TimeSlot)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(TimeSlot));

                if (timeslot.Id == 0)
                {
                    timeslot.IsDeleted = false;
                    timeslot.CreatedDate = DateTime.Now;
                    timeslot.ModifiedDate = DateTime.Now;
                    timeslot.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    timeslot.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.TimeSlots.Add(timeslot);
                }
                else
                {
                    timeslot.ModifiedDate = DateTime.Now;
                    timeslot.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(timeslot).State = EntityState.Modified;
                    db.Entry(timeslot).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(timeslot).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Time Slot Data";
            }

            return Ok(response);
        }

        public IHttpActionResult DeleteTimeSlot(int id)
        {
            var response = "OK";

            TimeSlot timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
            {
                response = "Time Slot Not Found";
            }

            timeslot.IsDeleted = true;
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