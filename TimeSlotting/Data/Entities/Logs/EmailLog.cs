using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Logs
{
    public class EmailLog
    {
        public int Id { get; set; }
        public int? WebUserId { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Attachment { get; set; }
        public string URL { get; set; }
        public string Filename { get; set; }
        public string SendError { get; set; }
        public DateTime SentDate { get; set; }
    }
}