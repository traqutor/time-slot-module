using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data;
using TimeSlotting.Data.Entities;
using TimeSlotting.Models;
using TimeSlotting.Models.Users;

namespace TimeSlotting.Services
{
    public class RoleService
    {
        private readonly int _pageSize = 20;
        private readonly TimeSlottingDBContext _context;

        public RoleService()
        {
            _context = TimeSlottingDBContext.Create();
        }

        public SearchResults<RoleListEntryViewModel> GetRoles(bool isAdmin, int? page)
        {
            var query = _context.Roles.OrderByDescending(r => r.Id).AsQueryable();

            if (!isAdmin)
                query = query.Where(role => role.Name != UserSystemRoles.Administrator);

            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);



            return new SearchResults<RoleListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new RoleListEntryViewModel(el)).ToList()
            };
        }
    }
}