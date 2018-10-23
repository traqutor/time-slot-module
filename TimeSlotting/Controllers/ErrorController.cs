using System.Web.Mvc;

namespace PrintCalculator.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult InternalServer()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}