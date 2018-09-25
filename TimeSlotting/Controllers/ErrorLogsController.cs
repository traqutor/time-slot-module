using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TimeSlotting.Models;
using System.Web.Mvc;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class ErrorLogsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ErrorLogs
        public IQueryable<ErrorLog> GetErrorLogs()
        {
            return db.ErrorLogs;
        }

        // GET: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult GetErrorLog(int id)
        {
            ErrorLog errorLog = db.ErrorLogs.Find(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            return Ok(errorLog);
        }

        // PUT: api/ErrorLogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutErrorLog(int id, ErrorLog errorLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != errorLog.Id)
            {
                return BadRequest();
            }

            db.Entry(errorLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ErrorLogs
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult PostErrorLog(ErrorLog errorLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ErrorLogs.Add(errorLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = errorLog.Id }, errorLog);
        }

        // DELETE: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult DeleteErrorLog(int id)
        {
            ErrorLog errorLog = db.ErrorLogs.Find(id);
            if (errorLog == null)
            {
                return NotFound();
            }
            else
            {
                db.ErrorLogs.Remove(errorLog);
                db.SaveChanges();
            }

            return Ok(errorLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ErrorLogExists(int id)
        {
            return db.ErrorLogs.Count(e => e.Id == id) > 0;
        }
    }
}