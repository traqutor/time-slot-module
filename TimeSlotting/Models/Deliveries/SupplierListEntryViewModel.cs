﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Models.Deliveries
{
    public class SupplierListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public SupplierListEntryViewModel()
        {

        }

        public SupplierListEntryViewModel(Supplier entity)
        {
            Id = entity.Id;
            Name = entity.Name;

            EntityStatus = entity.EntityStatus;

            CreatedBy = new UserListEntryViewModel(entity.CreatedBy);
            ModifiedBy = new UserListEntryViewModel(entity.ModifiedBy);

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}