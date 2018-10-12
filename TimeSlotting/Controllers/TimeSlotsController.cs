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
using TimeSlotting.Models.Deliveries;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class TimeSlotsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [ResponseType(typeof(List<TimeSlotListEntryViewModel>))]
        public IHttpActionResult GetTimeSlots()
        {
            return Ok(db.TimeSlots.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.StartTime).ToList().Select(el => new TimeSlotListEntryViewModel(el)).ToList());
        }

        [ResponseType(typeof(TimeSlotListEntryViewModel))]
        public IHttpActionResult GetTimeSlot(int id)
        {
            TimeSlot timeslot = db.TimeSlots.Find(id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return Ok(new TimeSlotListEntryViewModel(timeslot));
        }

        [ResponseType(typeof(TimeSlotListEntryViewModel))]
        public IHttpActionResult PutTimeSlot(TimeSlotListEntryViewModel model)
        {

            TimeSlot timeslot = new TimeSlot();

            if (model.Id == 0)
            {
                timeslot.StartTime = model.StartTime;
                timeslot.EndTime = model.EndTime;
                timeslot.EntityStatus = model.EntityStatus;
                timeslot.CreationDate = DateTime.UtcNow;
                timeslot.ModificationDate = DateTime.UtcNow;
                timeslot.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                timeslot.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.TimeSlots.Add(timeslot);
            }
            else
            {
                timeslot = db.TimeSlots.Find(model.Id);

                timeslot.StartTime = model.StartTime;
                timeslot.EndTime = model.EndTime;
                timeslot.EntityStatus = model.EntityStatus;

                timeslot.ModificationDate = DateTime.UtcNow;
                timeslot.ModifiedById = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();

            timeslot = db.TimeSlots.Include(e => e.CreatedBy).Include(e => e.ModifiedBy).SingleOrDefault(c => c.Id == timeslot.Id);

            return Ok(timeslot);
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