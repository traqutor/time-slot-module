using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSlotting.Models
{
    [MetadataType(typeof(DeliveryTimeSlotMetaData))]
    public partial class DeliveryTimeSlot
    {
        private class DeliveryTimeSlotMetaData
        {
            [ForeignKey("DriverId")]
            public WebUser WebUser { get; set; }
        }
    }
}