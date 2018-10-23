using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeSlotting.Data.Entities;
using TimeSlotting.Models;
using TimeSlotting.Models.Users;
using TimeSlotting.Services;

namespace TimeSlotting.Controllers
{
    public class RoleController : ApiController
    {
        private readonly RoleService _service;

        public RoleController()
        {
            _service = new RoleService();
        }


        public SearchResults<RoleListEntryViewModel> GetRoles(int? page = null)
        {
            if (User.IsInRole(UserSystemRoles.Administrator))
                return _service.GetRoles(true, page);
            else
                return _service.GetRoles(false, page);
        }
    }
}