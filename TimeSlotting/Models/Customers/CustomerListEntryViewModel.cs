using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Customers;

namespace TimeSlotting.Models.Customers
{
    public class CustomerListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public CustomerListEntryViewModel()
        {

        }

        public CustomerListEntryViewModel(Customer entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}