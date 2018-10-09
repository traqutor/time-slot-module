using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Data.Entities.Customers.Fleets
{
    public class Vehicle
    {
        public Vehicle()
        {
            this.VehicleDrivers = new HashSet<VehicleDriver>();
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
        }

        public int Id { get; set; }
        public string Rego { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int FleetId { get; set; }
        [ForeignKey("FleetId")]
        public virtual Fleet Fleet { get; set; }

        public virtual ICollection<VehicleDriver> VehicleDrivers { get; set; }
        public virtual ICollection<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }


        public EntityStatus EntityStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}