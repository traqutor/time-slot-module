using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TimeSlotting.Data.Initializers
{
    public class TimeSlottingDBContextInitializer : CreateDatabaseIfNotExists<TimeSlottingDBContext>
    {
        protected override void Seed(TimeSlottingDBContext context)
        {
        }
    }
}