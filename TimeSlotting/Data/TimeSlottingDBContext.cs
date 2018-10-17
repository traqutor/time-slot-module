using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities;
using TimeSlotting.Data.Initializers;
using System.Data.Entity;
using TimeSlotting.Data.Entities.Deliveries;
using TimeSlotting.Data.Entities.Customers;
using TimeSlotting.Data.Entities.Logs;
using TimeSlotting.Data.Entities.Customers.Fleets;

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

            modelBuilder.Entity<WebUser>()
                .HasOptional(u => u.CreatedBy).WithMany();

            modelBuilder.Entity<WebUser>()
                .HasOptional(u => u.ModifiedBy).WithMany();
        }

        public virtual DbSet<WebUser> WebUsers { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<Fleet> Fleets { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleDriver> VehicleDrivers { get; set; }
        
        public virtual DbSet<DeliveryTimeSlot> DeliveryTimeSlots { get; set; }
        public virtual DbSet<TimeSlot> TimeSlots { get; set; }
        public virtual DbSet<StatusType> StatusTypes { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        








        //Logs support stuff, why not using nlog? are these logs supposed to be displayed in application?
        public virtual DbSet<EmailLog> EmailLogs { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    }
}