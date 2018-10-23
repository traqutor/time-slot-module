using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Models.Deliveries
{
    public class ContractListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public VendorListEntryViewModel Vendor { get; set; }
        public SupplierListEntryViewModel Supplier { get; set; }
        public CommodityListEntryViewModel Commodity { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public ContractListEntryViewModel()
        {

        }

        public ContractListEntryViewModel(Contract entity)
        {
            Id = entity.Id;
            Name = entity.Name;

            EntityStatus = entity.EntityStatus;

            Vendor = new VendorListEntryViewModel(entity.Vendor);
            Supplier = new SupplierListEntryViewModel(entity.Supplier);
            Commodity = new CommodityListEntryViewModel(entity.Commodity);

            CreatedBy = new UserListEntryViewModel(entity.CreatedBy);
            ModifiedBy = new UserListEntryViewModel(entity.ModifiedBy);

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}