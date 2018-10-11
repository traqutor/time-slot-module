using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using System.Collections.Generic;
using TimeSlotting.Models.Deliveries;

namespace TimeSlotting.Controllers
{
    public class CommoditiesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        //[System.Web.Mvc.Authorize(Roles = "Administrator")]
        //[System.Web.Http.Authorize(Roles = "Administrator")]
        /// <summary>
        /// doesnt display deleted
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<CommodityListEntryViewModel>))]
        public IHttpActionResult GetCommodities()
        {
            return Ok(db.Commodities.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new CommodityListEntryViewModel(el)).ToList());
        }

        /// <summary>
        /// displays only normal (no deleted and disabled)
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<CommodityListEntryViewModel>))]
        public IHttpActionResult GetCommodityList()
        {
            return Ok(db.Commodities.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList().Select(el => new CommodityListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(CommodityListEntryViewModel))]
        public IHttpActionResult GetCommodity(int id)
        {
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return NotFound();
            }

            return Ok(new CommodityListEntryViewModel(commodity));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(CommodityListEntryViewModel))]
        public IHttpActionResult PutCommodity(CommodityListEntryViewModel model)
        {
            Commodity commodity = new Commodity();

            if (model.Id == 0)
            {
                commodity.Name = model.Name;
                commodity.EntityStatus = model.EntityStatus;
                commodity.CreationDate = DateTime.UtcNow;
                commodity.ModificationDate = DateTime.UtcNow;
                commodity.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                commodity.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Commodities.Add(commodity);
            }
            else
            {
                commodity = db.Commodities.Find(model.Id);

                commodity.Name = model.Name;
                commodity.EntityStatus = model.EntityStatus;

                commodity.ModificationDate = DateTime.UtcNow;
                commodity.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();
      
            return Ok(commodity);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteCommodity(int id)
        {
            var response = "OK";

            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                response = "Commodity Not Found";
            }
            else
            {
                commodity.EntityStatus = EntityStatus.DELETED;
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