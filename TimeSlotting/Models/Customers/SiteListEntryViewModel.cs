﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Customers;

namespace TimeSlotting.Models.Customers
{
    public class SiteListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CustomerListEntryViewModel Customer { get; set; }

        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        
        public EntityStatus EntityStatus { get; set; }


        public SiteListEntryViewModel()
        {

        }

        public SiteListEntryViewModel(Site entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Customer = new CustomerListEntryViewModel(entity.Customer);

            EntityStatus = entity.EntityStatus;

            CreatedBy = entity.CreatedBy;
            ModifiedBy = entity.ModifiedBy;

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}