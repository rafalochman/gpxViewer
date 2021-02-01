using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpxViewer.Models
{
    public class GpxData
    {
        public List<double> Lat { get; set; }
        public List<double> Lng { get; set; }
        public List<double> Distances { get; set; }
        public List<double> Elevations { get; set; }
    }
}