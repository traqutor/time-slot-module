using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers;
using TimeSlotting.Data.Entities.Customers.Fleets;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Data.Entities
{
    public class WebUser
    {
        public WebUser()
        {
            this.VehicleDrivers = new HashSet<VehicleDriver>();
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
        }

        public int Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ASPId { get; set; }
        [ForeignKey("ASPId")]
        public virtual User User { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? FleetId { get; set; }
        [ForeignKey("FleetId")]
        public virtual Fleet Fleet { get; set; }

        public int? SiteId { get; set; }
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public virtual ICollection<VehicleDriver> VehicleDrivers { get; set; }
        public virtual ICollection<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }


        public EntityStatus EntityStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual WebUser CreatedBy { get; set; }
        public int? ModifiedById { get; set; }
        [ForeignKey("ModifiedById")]
        public virtual WebUser ModifiedBy { get; set; }
    }
}