using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeSlotting.Data.Entities.Deliveries;

namespace TimeSlotting.Models.Deliveries
{
    public class TimeSlotWithDeliveryModel
    {
        public TimeSlotListEntryViewModel TimeSlot { get; set; }
        public DeliveryTimeSlotModel DeliveryTimeSlot { get; set; }

        public TimeSlotWithDeliveryModel()
        {

        }

        public TimeSlotWithDeliveryModel(TimeSlot ts, DeliveryTimeSlot deliveryTimeSlot)
        {
            TimeSlot = new TimeSlotListEntryViewModel(ts);
            DeliveryTimeSlot = new DeliveryTimeSlotModel(deliveryTimeSlot);
        }
    }
}