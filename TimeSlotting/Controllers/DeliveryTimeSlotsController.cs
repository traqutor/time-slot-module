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
    [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
    [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
    public class DeliveryTimeSlotsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        
        [ResponseType(typeof(List<DeliveryTimeSlotModel>))]
        public IHttpActionResult GetTimeSlots()
        {
            var timeslots = db.DeliveryTimeSlots.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.TimeSlot.StartTime).ToList().Select(c => new DeliveryTimeSlotModel(c)).ToList();

            return Ok(timeslots);
        }

        /// <summary>
        /// returns the list of deliveryTimeslotModels
        /// </summary>
        /// <param name="sid">Customer site id</param>
        /// <param name="day">date</param>
        /// <returns></returns>
        [ResponseType(typeof(List<DeliveryTimeSlotModel>))]
        public IHttpActionResult GetTimeSlotData(int sid, string day)
        {
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
                             where m.EntityStatus == EntityStatus.NORMAL
                             select m).OrderBy(x => x.StartTime).ToList();

            int[] timeslotsArr = timeslots.Select(ts => ts.Id).ToArray();

           
            var timeslot = (from m in db.DeliveryTimeSlots
                            where m.EntityStatus != EntityStatus.DELETED && timeslotsArr.Contains(m.TimeSlotId)
                            && m.SiteId == sid && m.DeliveryDate == DbFunctions.TruncateTime(dayDate.Date)
                            select m).ToList();

            return Ok(timeslot.Select(el => new DeliveryTimeSlotModel(el)));
        }

        [ResponseType(typeof(DeliveryTimeSlotModel))]
        public IHttpActionResult GetTimeSlot(int id)
        {
            return Ok(new DeliveryTimeSlotModel(db.DeliveryTimeSlots.Find(id)));
        }

        public IHttpActionResult GetTimeSlot(int tid, int sid, DateTime day)
        {
            var timeslot = db.DeliveryTimeSlots.Where(x => x.EntityStatus != EntityStatus.DELETED && x.TimeSlotId == tid && x.SiteId == sid && x.DeliveryDate == day.Date).ToList();
            if (timeslot.Count > 0)
            {
                return NotFound();
            }

            return Ok(timeslot);
        }

        /// <summary>
        /// For DelivetyTimeslot creation/edition
        /// </summary>
        /// <param name="model">provide: deliveryDate, TimeSlot.Id, Customer.Id, Site.Id, StatusType.Id, Contract.Id, Supplier.Id, Vendor.Id, Commodity.Id, Vehicle.Id, Driver.Id </param>
        /// <returns></returns>
        [ResponseType(typeof(DeliveryTimeSlotModel))]
        public IHttpActionResult PutTimeSlot(DeliveryTimeSlotModel model)
        {
            DeliveryTimeSlot timeslot = new DeliveryTimeSlot();

            if (model.Id == 0)
            {

                timeslot.Tons = model.Tons;
                timeslot.DeliveryDate = model.DeliveryDate;

                timeslot.TimeSlotId = model.TimeSlot.Id;
                
                timeslot.CustomerId = model.Customer.Id;
                timeslot.SiteId = model.Site.Id;
                timeslot.StatusTypeId = model.StatusType.Id;

                timeslot.ContractId = model.Contract.Id;
                timeslot.SupplierId = model.Supplier.Id;
                timeslot.VendorId = model.Vendor.Id;
                timeslot.CommodityId = model.Commodity.Id;

                timeslot.VehicleId = model.Vehicle.Id;
                timeslot.DriverId = model.Driver.Id;

                timeslot.EntityStatus = EntityStatus.NORMAL;
                timeslot.CreationDate = DateTime.UtcNow;
                timeslot.ModificationDate = DateTime.UtcNow;
                timeslot.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                timeslot.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.DeliveryTimeSlots.Add(timeslot);
            }
            else
            {
                timeslot = db.DeliveryTimeSlots.Find(model.Id);

                timeslot.Tons = model.Tons;
                timeslot.DeliveryDate = model.DeliveryDate;

                timeslot.TimeSlotId = model.TimeSlot.Id;

                timeslot.CustomerId = model.Customer.Id;
                timeslot.SiteId = model.Site.Id;
                timeslot.StatusTypeId = model.StatusType.Id;

                timeslot.ContractId = model.Contract.Id;
                timeslot.SupplierId = model.Supplier.Id;
                timeslot.VendorId = model.Vendor.Id;
                timeslot.CommodityId = model.Commodity.Id;

                timeslot.VehicleId = model.Vehicle.Id;
                timeslot.DriverId = model.Driver.Id;

                timeslot.ModificationDate = DateTime.UtcNow;
                timeslot.ModifiedById = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();

            //have to make a new query with something included so it doest use the cached one -- need a better solution for that
            timeslot = db.DeliveryTimeSlots.Include(dts => dts.Customer).SingleOrDefault(dts => dts.Id == timeslot.Id);
            //db.Entry(site).Reload(); -nope
            //db.Entry(site).GetDatabaseValues(); -nope

            return Ok(new DeliveryTimeSlotModel(timeslot));
        }

        public IHttpActionResult DeleteTimeSlot(int id)
        {
            var response = "OK";

            DeliveryTimeSlot timeslot = db.DeliveryTimeSlots.Find(id);
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