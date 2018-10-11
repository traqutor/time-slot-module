using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Models.Deliveries
{
    public class ContractListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public ContractListEntryViewModel()
        {

        }

        public ContractListEntryViewModel(Contract entity)
        {
            Id = entity.Id;
            Name = entity.Name;

            EntityStatus = entity.EntityStatus;

            CreatedBy = entity.CreatedBy;
            ModifiedBy = entity.ModifiedBy;

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}