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
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Deliveries;
using System.Collections.Generic;

namespace TimeSlotting.Controllers
{
    public class ContractsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<ContractListEntryViewModel>))]
        public IHttpActionResult GetContracts()
        {
            return Ok(db.Contracts.Where(x => x.EntityStatus != EntityStatus.DELETED).OrderBy(x => x.Name).ToList().Select(el => new ContractListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [System.Web.Http.Authorize(Roles = "Administrator, CustomerAdmin, CustomerUser, SiteUser, Driver")]
        [ResponseType(typeof(List<ContractListEntryViewModel>))]
        public IHttpActionResult GetContractList()
        {
            return Ok(db.Contracts.Where(x => x.EntityStatus == EntityStatus.NORMAL).OrderBy(x => x.Name).ToList().Select(el => new ContractListEntryViewModel(el)).ToList());
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(ContractListEntryViewModel))]
        public IHttpActionResult GetContract(int id)
        {
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return NotFound();
            }

            return Ok(new ContractListEntryViewModel(contract));
        }

        [System.Web.Mvc.Authorize(Roles = "Administrator")]
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [ResponseType(typeof(ContractListEntryViewModel))]
        public IHttpActionResult PutContract(ContractListEntryViewModel model)
        {
            Contract contract = new Contract();

            if (model.Id == 0)
            {
                contract.Name = model.Name;
                contract.EntityStatus = model.EntityStatus;
                contract.CreationDate = DateTime.UtcNow;
                contract.ModificationDate = DateTime.UtcNow;
                contract.CreatedBy = Common.GetUserId(User.Identity.GetUserId());
                contract.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());

                db.Contracts.Add(contract);
            }
            else
            {
                contract = db.Contracts.Find(model.Id);

                contract.Name = model.Name;
                contract.EntityStatus = model.EntityStatus;
                contract.ModificationDate = DateTime.UtcNow;
                contract.ModifiedBy = Common.GetUserId(User.Identity.GetUserId());
            }

            db.SaveChanges();
       
            return Ok(contract);
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
                contract.EntityStatus = EntityStatus.DELETED;
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