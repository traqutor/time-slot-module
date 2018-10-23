using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TimeSlotting.Models;
using System.Web.Mvc;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities.Logs;

namespace TimeSlotting.Controllers
{
    [HandleError]
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class EmailLogsController : ApiController
    {
        private TimeSlottingDBContext db = new TimeSlottingDBContext();

        // GET: api/EmailLogs
        public IQueryable<EmailLog> GetEmailLogs()
        {
            return db.EmailLogs;
        }

        // GET: api/EmailLogs/5
        [ResponseType(typeof(EmailLog))]
        public IHttpActionResult GetEmailLog(int id)
        {
            EmailLog emailLog = db.EmailLogs.Find(id);
            if (emailLog == null)
            {
                return NotFound();
            }

            return Ok(emailLog);
        }

        // PUT: api/EmailLogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmailLog(int id, EmailLog emailLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailLog.Id)
            {
                return BadRequest();
            }

            db.Entry(emailLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailLogExists(id))
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

        // POST: api/EmailLogs
        [ResponseType(typeof(EmailLog))]
        public IHttpActionResult PostEmailLog(EmailLog emailLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmailLogs.Add(emailLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = emailLog.Id }, emailLog);
        }

        // DELETE: api/EmailLogs/5
        [ResponseType(typeof(EmailLog))]
        public IHttpActionResult DeleteEmailLog(int id)
        {
            EmailLog emailLog = db.EmailLogs.Find(id);
            if (emailLog == null)
            {
                return NotFound();
            }
            else
            {
                db.EmailLogs.Remove(emailLog);
                db.SaveChanges();
            }

            return Ok(emailLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailLogExists(int id)
        {
            return db.EmailLogs.Count(e => e.Id == id) > 0;
        }
    }
}