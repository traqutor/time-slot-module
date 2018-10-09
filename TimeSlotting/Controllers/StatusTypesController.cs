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

namespace TimeSlotting.Controllers
{
    public class StatusTypesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult GetStatusTypes()
        {
            return Ok(db.StatusTypes.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetStatusTypeList()
        {
            return Ok(db.StatusTypes.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult GetStatusType(int id)
        {
            StatusType statusType = db.StatusTypes.Find(id);
            if (statusType == null)
            {
                return NotFound();
            }

            return Ok(statusType);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutStatusType(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                StatusType statusType = (StatusType)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(StatusType));

                if (statusType.Id == 0)
                {
                    statusType.EntityStatus = EntityStatus.NORMAL;
                    statusType.CreationDate = DateTime.UtcNow;
                    statusType.ModificationDate = DateTime.UtcNow;
                    statusType.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    statusType.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.StatusTypes.Add(statusType);
                }
                else
                {
                    statusType.ModificationDate = DateTime.UtcNow;
                    statusType.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(statusType).State = EntityState.Modified;
                    db.Entry(statusType).Property(x => x.CreationDate).IsModified = false;
                    db.Entry(statusType).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Status Type Data";
            }

            return Ok(response);
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