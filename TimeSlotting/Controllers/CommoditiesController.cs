using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using TimeSlotting.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace TimeSlotting.Controllers
{
    public class CommoditiesController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        //[System.Web.Mvc.Authorize(Roles = "Administrator")]
        //[System.Web.Http.Authorize(Roles = "Administrator")]
        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetCommodities()
        {
            return Ok(db.Commodities.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetCommodityList()
        {
            return Ok(db.Commodities.Where(x => !x.IsDeleted && x.IsEnabled).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(Commodity))]
        public IHttpActionResult GetCommodity(int id)
        {
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return NotFound();
            }

            return Ok(commodity);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutCommodity(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Commodity commodity = (Commodity)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Commodity));

                if (commodity.Id == 0)
                {
                    commodity.IsDeleted = false;
                    commodity.CreatedDate = DateTime.Now;
                    commodity.ModifiedDate = DateTime.Now;
                    commodity.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    commodity.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Commodities.Add(commodity);
                }
                else
                {
                    commodity.ModifiedDate = DateTime.Now;
                    commodity.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(commodity).State = EntityState.Modified;
                    db.Entry(commodity).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(commodity).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Commodity Data";
            }

            return Ok(response);
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
                commodity.IsDeleted = true;
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