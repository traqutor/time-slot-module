using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Models.Deliveries
{
    public class UsedCommodityModel
    {
        public int UsedAmount { get; set; }

        public UsedCommodityModel(int? sum)
        {
            if (sum.HasValue)
                UsedAmount = sum.Value;
        }
    }
}