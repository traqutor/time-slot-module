using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Logs
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string URL { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}