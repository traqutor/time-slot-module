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
    public class ContractsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult GetContracts()
        {
            return Ok(db.Contracts.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        public IHttpActionResult GetContractList()
        {
            return Ok(db.Contracts.Where(x => !x.IsDeleted && x.IsEnabled).OrderBy(x => x.Name).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(Contract))]
        public IHttpActionResult GetContract(int id)
        {
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult PutContract(JObject jsonResult)
        {
            var response = "OK";

            if (jsonResult != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                Contract contract = (Contract)serializer.Deserialize(new JTokenReader(jsonResult.First.First), typeof(Contract));

                if (contract.Id == 0)
                {
                    contract.IsDeleted = false;
                    contract.CreatedDate = DateTime.Now;
                    contract.ModifiedDate = DateTime.Now;
                    contract.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                    contract.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Contracts.Add(contract);
                }
                else
                {
                    contract.ModifiedDate = DateTime.Now;
                    contract.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                    db.Entry(contract).State = EntityState.Modified;
                    db.Entry(contract).Property(x => x.CreatedDate).IsModified = false;
                    db.Entry(contract).Property(x => x.CreatedBy).IsModified = false;
                }

                db.SaveChanges();
            }
            else
            {
                response = "No Contract Data";
            }

            return Ok(response);
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteContract(int id)
        {
            var response = "OK";

            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                response = "Contract Not Found";
            }
            else
            {
                contract.IsDeleted = true;
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