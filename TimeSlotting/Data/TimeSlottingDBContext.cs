using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Initializers;
using System.Data.Entity;

namespace TimeSlotting.Data
{
    public class TimeSlottingDBContext : IdentityDbContext<User>
    {
        public TimeSlottingDBContext()
           : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new TimeSlottingDBContextInitializer());
        }

        public static TimeSlottingDBContext Create()
        {
            return new TimeSlottingDBContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}