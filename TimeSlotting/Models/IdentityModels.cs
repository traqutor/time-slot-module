using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeSlotting.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<TimeSlotting.Models.WebUser> WebUsers { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.EmailLog> EmailLogs { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.ErrorLog> ErrorLogs { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Fleet> Fleets { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Site> Sites { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.TimeSlot> TimeSlots { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Vehicle> Vehicles { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.VehicleDriver> VehicleDrivers { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.StatusType> StatusTypes { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.DeliveryTimeSlot> DeliveryTimeSlots { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Vendor> Vendors { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Contract> Contracts { get; set; }

        public System.Data.Entity.DbSet<TimeSlotting.Models.Commodity> Commodities { get; set; }
    }
}