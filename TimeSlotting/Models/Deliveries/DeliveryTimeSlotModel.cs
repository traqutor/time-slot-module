using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Customers;
using TimeSlotting.Models.Customers.Fleets;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Models.Deliveries
{
    public class DeliveryTimeSlotModel
    {
        public int Id { get; set; }
        public int? Tons { get; set; }

        public TimeSlotListEntryViewModel TimeSlot { get; set; }
        public DateTime DeliveryDate { get; set; }

        public StatusTypeListEntryViewModel StatusType { get; set; }

        public ContractListEntryViewModel Contract { get; set; }

        public CommodityListEntryViewModel Commodity { get; set; }
        public SupplierListEntryViewModel Supplier { get; set; }
        public VendorListEntryViewModel Vendor { get; set; }

        public CustomerListEntryViewModel Customer { get; set; }
        public SiteListEntryViewModel Site { get; set; }

        public VehicleListEntryViewModel Vehicle { get; set; }
        public UserListEntryViewModel Driver { get; set; }


        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public DeliveryTimeSlotModel()
        {

        }

        public DeliveryTimeSlotModel(DeliveryTimeSlot entity)
        {
            Id = entity.Id;
            Tons = entity.Tons;

            TimeSlot = new TimeSlotListEntryViewModel(entity.TimeSlot);
            DeliveryDate = entity.DeliveryDate;

            StatusType = new StatusTypeListEntryViewModel(entity.StatusType);

            Contract = new ContractListEntryViewModel(entity.Contract);

            Commodity = new CommodityListEntryViewModel(entity.Commodity);
            Supplier = new SupplierListEntryViewModel(entity.Supplier);
            Vendor = new VendorListEntryViewModel(entity.Vendor);

            Customer = new CustomerListEntryViewModel(entity.Customer);
            Site = new SiteListEntryViewModel(entity.Site);

            Vehicle = new VehicleListEntryViewModel(entity.Vehicle);
            Driver = new UserListEntryViewModel(entity.WebUser);


            EntityStatus = entity.EntityStatus;

            CreatedBy = new UserListEntryViewModel(entity.CreatedBy);
            ModifiedBy = new UserListEntryViewModel(entity.ModifiedBy);

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }

    }
}