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
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class TimeSlotsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        public IHttpActionResult GetTimeSlots()
        {
            return Ok(db.TimeSlots.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.StartTime).ToList());
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
                    timeslot.EntityStatus = EntityStatus.NORMAL;
                    timeslot.CreationDate = DateTime.UtcNow;
                    timeslot.ModificationDate = DateTime.UtcNow;
                    timeslot.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    timeslot.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.TimeSlots.Add(timeslot);
                }
                else
                {
                    timeslot.ModificationDate = DateTime.UtcNow;
                    timeslot.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(timeslot).State = EntityState.Modified;
                    db.Entry(timeslot).Property(x => x.CreationDate).IsModified = false;
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
            else
            {
                timeslot.EntityStatus = EntityStatus.DELETED;
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