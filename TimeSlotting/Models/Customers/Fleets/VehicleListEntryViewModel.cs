using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Customers.Fleets;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Models.Customers.Fleets
{
    public class VehicleListEntryViewModel
    {
        public int Id { get; set; }
        public string Rego { get; set; }

        //public CustomerListEntryViewModel Customer  { get; set; }
        public FleetListEntryViewModel Fleet { get; set; }

        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public VehicleListEntryViewModel()
        {

        }

        public VehicleListEntryViewModel(Vehicle entity)
        {
            Id = entity.Id;
            Rego = entity.Rego;

            Fleet = new FleetListEntryViewModel(entity.Fleet);
            //Customer = new CustomerListEntryViewModel(entity.Customer);

            EntityStatus = entity.EntityStatus;

            CreatedBy = new UserListEntryViewModel(entity.CreatedBy);
            ModifiedBy = new UserListEntryViewModel(entity.ModifiedBy);

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}