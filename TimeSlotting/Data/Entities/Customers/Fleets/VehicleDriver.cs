using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Customers.Fleets
{
    public class VehicleDriver
    {
        public int Id { get; set; }
        
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        public int WebUserId { get; set; }
        [ForeignKey("WebUserId")]
        public virtual WebUser WebUser { get; set; }
    }
}