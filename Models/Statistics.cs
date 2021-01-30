using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpxViewer.Models
{
    public class Statistics
    {
        public int RidesNumber { get; set; }
        public double Distance { get; set; }
        public double Elevation { get; set; }
        public TimeSpan Time { get; set; }
    }
}