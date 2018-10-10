using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers.Fleets;

namespace TimeSlotting.Models.Customers.Fleets
{
    public class FleetListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public FleetListEntryViewModel()
        {

        }

        public FleetListEntryViewModel(Fleet entity)
        {

        }
    }
}