using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSlotting.Models
{
    public class SearchResults<T>
    {
        public int ResultsCount { get; set; }
        public List<T> Results { get; set; }
    }
}