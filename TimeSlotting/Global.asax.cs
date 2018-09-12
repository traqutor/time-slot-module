using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using TimeSlotting.Models;

namespace TimeSlotting
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            var newError = new ErrorLog();
            newError.Message = ex.Message;
            newError.StackTrace = ex.StackTrace;
            newError.URL = Request.RawUrl;
            newError.CreatedDate = DateTime.Now;
            newError.CreatedBy = Common.GetUserId(User.Identity.GetUserId());

            if (ex is DbEntityValidationException)
            {
                DbEntityValidationException dbevex = ex as DbEntityValidationException;

                var errorMessages = (from eve in dbevex.EntityValidationErrors
                                     let entity = eve.Entry.Entity.GetType().Name
                                     from ev in eve.ValidationErrors
                                     select new { Entity = entity, PropertyName = ev.PropertyName, ErrorMessage = ev.ErrorMessage });

                var fullErrorMessage = string.Join("; ", errorMessages.Select(x => string.Format("[Entity: {0}, Property: {1}] {2}", x.Entity, x.PropertyName, x.ErrorMessage)));
                var exceptionMessage = string.Concat(dbevex.Message, " The validation errors are: ", fullErrorMessage);

                newError.Message = fullErrorMessage;
                newError.StackTrace = exceptionMessage;
            }

            ApplicationDbContext db = new ApplicationDbContext();
            db.ErrorLogs.Add(newError);
            db.SaveChanges();
            db.Dispose();
        }
    }
}
