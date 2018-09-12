//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeSlotting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WebUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebUser()
        {
            this.DeliveryTimeSlots = new HashSet<DeliveryTimeSlot>();
            this.VehicleDrivers = new HashSet<VehicleDriver>();
        }
    
        public int Id { get; set; }
        public string ASPId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> SiteId { get; set; }
        public Nullable<int> FleetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }
        public virtual Fleet Fleet { get; set; }
        public virtual Site Site { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleDriver> VehicleDrivers { get; set; }
    }
}
