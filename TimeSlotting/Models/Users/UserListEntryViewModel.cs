using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;

namespace TimeSlotting.Models.Users
{
    public class UserListEntryViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }

        public UserListEntryViewModel()
        {

        }

        public UserListEntryViewModel(WebUser entity, List<IdentityRole> roles)
        {
            Id = entity.Id;
            Email = entity.User.UserName;

            if (entity.Customer != null)
            {
                //Customer = new CustomerListEntryViewModel(entity.Customer);
                //Customer.CustomerSites = entity.CustomerSiteUser.Select(el => new CustomerSiteListEntryViewModel(el.CustomerSite)).ToList();
            }

            //if (entity.FarmUsers != null)
              //  Farms = entity.FarmUsers.Select(fu => new FarmListEntryViewModel(fu.Farm)).ToList();

            //Role = new RoleListEntryViewModel(roles.SingleOrDefault(r => r.Id == entity.Roles.FirstOrDefault().RoleId));
        }
    }
}