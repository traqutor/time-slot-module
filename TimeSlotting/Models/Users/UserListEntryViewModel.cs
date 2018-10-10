using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Models.Customers;

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
            {
                Customer = new CustomerListEntryViewModel(entity.Customer);
            }

            if (entity.Site != null)
            {
                Site = new SiteListEntryViewModel(entity.Site);
            }

            Role = new RoleListEntryViewModel(roles.SingleOrDefault(r => r.Id == entity.User.Roles.FirstOrDefault().RoleId));

            EntityStatus = entity.EntityStatus;
        }
    }
}