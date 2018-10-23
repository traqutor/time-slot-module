using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities
{
    public class UserSystemRoles
    {
        public static string Administrator = "Administrator";
        public static string CustomerAdmin = "CustomerAdmin";
        public static string CustomerUser = "CustomerUser";
        public static string SiteUser = "SiteUser";
        public static string Driver = "Driver";

        /// <summary>
        /// used for database initialization, if adding new roles, please assign it here so the initializer will create all roles properly
        /// </summary>
        public static List<string> PossibleRoles = new List<string> { Administrator, CustomerAdmin, CustomerUser, SiteUser,Driver };
    }
}