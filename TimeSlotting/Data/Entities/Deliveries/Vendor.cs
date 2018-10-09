using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Deliveries
{
    public class Vendor
    {
        public Vendor()
        {
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }

        public EntityStatus EntityStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}