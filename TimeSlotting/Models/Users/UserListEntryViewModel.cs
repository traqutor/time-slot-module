using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Models.Customers;
using TimeSlotting.Models.Customers.Fleets;

namespace TimeSlotting.Models.Users
{
    public class UserListEntryViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public RoleListEntryViewModel Role { get; set; }

        public CustomerListEntryViewModel Customer { get; set; }
        public SiteListEntryViewModel Site { get; set; }
        public FleetListEntryViewModel Fleet { get; set; }
        public List<VehicleListEntryViewModel> Vehicles { get; set; } = new List<VehicleListEntryViewModel>();

        public EntityStatus EntityStatus { get; set; }

        public UserListEntryViewModel()
        {

        }

        public UserListEntryViewModel(WebUser entity, List<IdentityRole> roles)
        {
            Id = entity.Id;
            Email = entity.User.UserName;

            Name = entity.FirstName;
            Surname = entity.LastName;

            if (entity.Customer != null)
                Customer = new CustomerListEntryViewModel(entity.Customer);
          
            if (entity.Site != null)
                Site = new SiteListEntryViewModel(entity.Site);
           
            if (entity.Fleet != null)
                Fleet = new FleetListEntryViewModel(entity.Fleet);

            if(entity.VehicleDrivers != null)
            {
                foreach (var item in entity.VehicleDrivers)
                {
                    if (item.Vehicle != null)
                        Vehicles.Add(new VehicleListEntryViewModel(item.Vehicle));
                }
            }

            Role = new RoleListEntryViewModel(roles.SingleOrDefault(r => r.Id == entity.User.Roles.FirstOrDefault().RoleId));

            EntityStatus = entity.EntityStatus;
        }
    }
}