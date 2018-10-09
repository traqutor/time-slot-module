using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Entities
{
    public class UserSystemRoles
    {
        public string Administrator = "Administrator";
        public static string CustomerAdmin = "CustomerAdmin";
        public static string CustomerUser = "CustomerUser";
        public static string SiteUser = "SiteUser";
        public static string Driver = "Driver";
    }
}