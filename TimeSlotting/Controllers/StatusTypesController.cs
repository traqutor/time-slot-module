using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using TimeSlotting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Deliveries;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace TimeSlotting.Controllers
{
    public class StatusTypesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(List<StatusTypeListEntryViewModel>))]
        public IHttpActionResult GetStatusTypes()
        {
            return Ok(db.StatusTypes.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new StatusTypeListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<StatusTypeListEntryViewModel>))]
        public IHttpActionResult GetStatusTypeList()
        {
            return Ok(db.StatusTypes.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList().Select(el => new StatusTypeListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(StatusTypeListEntryViewModel))]
        public IHttpActionResult GetStatusType(int id)
        {
            StatusType statusType = db.StatusTypes.Find(id);
            if (statusType == null)
            {
                return NotFound();
            }

            return Ok(new StatusTypeListEntryViewModel(statusType));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(StatusTypeListEntryViewModel))]
        public IHttpActionResult PutStatusType(StatusTypeListEntryViewModel model)
        {
            
            StatusType statusType = new StatusType();

            if (model.Id == 0)
            {
                statusType.Name = model.Name;
                statusType.EntityStatus = model.EntityStatus;
                statusType.CreationDate = DateTime.UtcNow;
                statusType.ModificationDate = DateTime.UtcNow;
                statusType.CreatedById = Common.GetUserId(User.Identity.GetUserId());
                statusType.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.StatusTypes.Add(statusType);
            }
            else
            {
                statusType = db.StatusTypes.Find(model.Id);

                statusType.Name = model.Name;
                statusType.EntityStatus = model.EntityStatus;
                statusType.ModificationDate = DateTime.UtcNow;
                statusType.ModifiedById = Common.GetUserId(User.Identity.GetUserId());

                db.Entry(statusType).State = EntityState.Modified;
                db.Entry(statusType).Property(x => x.CreationDate).IsModified = false;
                db.Entry(statusType).Property(x => x.CreatedBy).IsModified = false;
            }

            db.SaveChanges();

            statusType = db.StatusTypes.Include(e => e.CreatedBy).Include(e => e.ModifiedBy).SingleOrDefault(c => c.Id == statusType.Id);

            return Ok(statusType);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteStatusType(int id)
        {
            var response = "OK";

            StatusType statusType = db.StatusTypes.Find(id);
            if (statusType == null)
            {
                response = "Status Type Not Found";
            }
            else
            {
                statusType.EntityStatus = EntityStatus.DELETED;
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