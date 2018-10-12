using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers.Fleets;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Data.Entities.Customers
{
    public class Customer
    {
        public Customer()
        {
            this.Sites = new HashSet<Site>();
            this.Fleets = new HashSet<Fleet>();
            this.Vehicles = new HashSet<Vehicle>();
            this.WebUsers = new HashSet<WebUser>();
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
        public virtual ICollection<Fleet> Fleets { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<WebUser> WebUsers { get; set; }
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