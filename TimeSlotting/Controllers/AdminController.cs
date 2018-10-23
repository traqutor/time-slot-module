using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [Authorize(Roles = "Administrator, CustomerAdmin")]
    public class AdminController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Customers()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, CustomerAdmin")]
        public ActionResult Sites()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, CustomerAdmin")]
        public ActionResult Fleets()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, CustomerAdmin")]
        public ActionResult Vehicles()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult TimeSlots()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Vendors()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Suppliers()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Contracts()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Commodities()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult StatusTypes()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, CustomerAdmin")]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EmailLog()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ErrorLog()
        {
            return View();
        }
    }
}