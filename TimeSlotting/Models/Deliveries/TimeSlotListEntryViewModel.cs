using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Models.Deliveries
{
    public class TimeSlotListEntryViewModel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public TimeSlotListEntryViewModel()
        {

        }

        public TimeSlotListEntryViewModel(TimeSlot entity)
        {
            Id = entity.Id;
            entity.StartTime = entity.StartTime;
            entity.EndTime = entity.EndTime;

            EntityStatus = entity.EntityStatus;

            CreatedBy = entity.CreatedBy;
            ModifiedBy = entity.ModifiedBy;

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}