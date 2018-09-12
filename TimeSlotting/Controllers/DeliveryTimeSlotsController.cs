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
    [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
    [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
    public class DeliveryTimeSlotsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetTimeSlots()
        {
            var timeslots = db.DeliveryTimeSlots.Where(x => !x.IsDeleted).OrderBy(x => x.TimeSlot.StartTime).ToList();

            return Ok(timeslots);
        }

        public IHttpActionResult GetTimeSlotData(int sid, string day)
        {
            //int sid = 0;
            DateTime dayDate = DateTime.Parse(day);

            var site = db.Sites.Find(sid);

            //var timeslots = (from m in db.TimeSlots
            //                 join d in db.DeliveryTimeSlots on m.Id equals d.TimeSlotId into LeftJoin
            //                 from dj in LeftJoin.DefaultIfEmpty()
            //                 where !m.IsDeleted && m.IsEnabled 
            //                 && (dj == null || (dj.SiteId == sid && dj.DeliveryDate == DbFunctions.TruncateTime(dayDate.Date)))
            //                 select new { m, dj }).OrderBy(x => x.m.StartTime).AsEnumerable()
            //                    .Select(c => Tuple.Create(c.m, (c.dj == null ? new DeliveryTimeSlot() : c.dj))).ToList();

            var timeslots = (from m in db.TimeSlots
                             where !m.IsDeleted && m.IsEnabled
                             select m).OrderBy(x => x.StartTime).ToList();

            List<Tuple<TimeSlot, DeliveryTimeSlot>> list = new List<Tuple<TimeSlot, DeliveryTimeSlot>>();
            foreach (TimeSlot item in timeslots)
            {
                var timeslot = (from m in db.DeliveryTimeSlots
                                where !m.IsDeleted && m.TimeSlotId == item.Id
                                && m.SiteId == sid && m.DeliveryDate == DbFunctions.TruncateTime(dayDate.Date)
                                select m).ToList();

                if (timeslot.Count > 0)
                {
                    list.Add(new Tuple<TimeSlot, DeliveryTimeSlot>(item, timeslot[0]));
                }
                else
                {
                    list.Add(new Tuple<TimeSlot, DeliveryTimeSlot>(item, new DeliveryTimeSlot()));
                }
            }

            return Ok(new { data = list, cid = site.CustomerId });
        }

        public IHttpActionResult GetTimeSlot(int id)
        {
            return Ok(db.DeliveryTimeSlots.Find(id));
        }

        public IHttpActionResult GetTimeSlot(int tid, int sid, DateTime day)
        {
            var timeslot = db.DeliveryTimeSlots.Where(x => !x.IsDeleted && x.TimeSlotId == tid && x.SiteId == sid && x.DeliveryDate == day.Date).ToList();
            if (timeslot.Count > 0)
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
                DeliveryTimeSlot timeslot = (DeliveryTimeSlot)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(DeliveryTimeSlot));

                if (timeslot.Id == 0)
                {
                    timeslot.IsDeleted = false;
                    timeslot.CreatedDate = DateTime.Now;
                    timeslot.ModifiedDate = DateTime.Now;
                    timeslot.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    timeslot.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.DeliveryTimeSlots.Add(timeslot);
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

            DeliveryTimeSlot timeslot = db.DeliveryTimeSlots.Find(id);
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