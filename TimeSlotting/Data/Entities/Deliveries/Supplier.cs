using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Deliveries
{
    public class Supplier
    {
        public Supplier()
        {
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
     
        public virtual ICollection<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }


        public EntityStatus EntityStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual WebUser CreatedBy { get; set; }
        public int ModifiedById { get; set; }
        [ForeignKey("ModifiedById")]
        public virtual WebUser ModifiedBy { get; set; }
    }
}