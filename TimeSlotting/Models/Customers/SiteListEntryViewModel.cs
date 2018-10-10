using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers;

namespace TimeSlotting.Models.Customers
{
    public class SiteListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SiteListEntryViewModel()
        {

        }

        public SiteListEntryViewModel(Site entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}