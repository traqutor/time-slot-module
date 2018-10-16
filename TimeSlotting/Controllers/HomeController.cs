

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TimeSlotting.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        [Route("")]
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            #if DEBUG

            response.Headers.Location = new Uri(Request.RequestUri + "/swagger");
            return response;

            #else

            response.Headers.Location = new Uri(Request.RequestUri + "/app");
            return response;
    
            #endif
        }
    }
}