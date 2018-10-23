﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Models.Deliveries
{
    public class TimeSlotListEntryViewModel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public TimeSlotListEntryViewModel()
        {

        }

        public TimeSlotListEntryViewModel(TimeSlot entity)
        {
            Id = entity.Id;
            StartTime = entity.StartTime;
            EndTime = entity.EndTime;

            EntityStatus = entity.EntityStatus;

            CreatedBy = new UserListEntryViewModel(entity.CreatedBy);
            ModifiedBy = new UserListEntryViewModel(entity.ModifiedBy);

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}