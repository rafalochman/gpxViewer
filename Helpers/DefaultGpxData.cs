using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpxViewer.Helpers
{
    public class DefaultGpxData
    {
        public GpxData GetDefaultGpxData()
        {
            GpxData gpxData = new GpxData
            {
                Lat = new List<double> { 51.91 },
                Lng = new List<double> { 19.13 },
                Distances = new List<double> { 0.0 },
                Elevations = new List<double> { 0.0 }
            };
            return gpxData;
        }
    }
}