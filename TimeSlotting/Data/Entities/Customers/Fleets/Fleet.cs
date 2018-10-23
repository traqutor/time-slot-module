using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities.Customers.Fleets
{
    public class Fleet
    {
        public Fleet()
        {
            this.Vehicles = new HashSet<Vehicle>();
            this.WebUsers = new HashSet<WebUser>();
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<WebUser> WebUsers { get; set; }


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