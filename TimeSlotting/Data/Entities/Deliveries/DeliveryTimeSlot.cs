using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers;
using TimeSlotting.Data.Entities.Customers.Fleets;

namespace TimeSlotting.Data.Entities.Deliveries
{
    public class DeliveryTimeSlot
    {
        public int Id { get; set; }

        public int? Tons { get; set; }
        public DateTime DeliveryDate { get; set; }
        
        public int ContractId { get; set; }
        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int SiteId { get; set; }
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public int StatusTypeId { get; set; }
        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public int TimeSlotId { get; set; }
        [ForeignKey("TimeSlotId")]
        public virtual TimeSlot TimeSlot { get; set; }

        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }

        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual WebUser WebUser { get; set; }

        public int CommodityId { get; set; }
        [ForeignKey("CommodityId")]
        public virtual Commodity Commodity { get; set; }


        public EntityStatus EntityStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}