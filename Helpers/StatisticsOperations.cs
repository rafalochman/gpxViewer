using gpxViewer.DataAccess;
using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Globalization;

namespace gpxViewer.Helpers
{
    public class StatisticsOperations
    {
        public Statistics Statistics { get; set; }
        
        public StatisticsOperations()
        {
            SetStatistics();
        }

        private void SetStatistics()
        {
            NumberFormatInfo numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberDecimalSeparator = ".";
            GpxContext db = new GpxContext();
            var routes = db.GpxRoutes.ToList();

            double distance = 0;
            double elevation = 0;
            TimeSpan time = new TimeSpan(0, 0, 0, 0, 0);

            foreach(GpxRoute route in routes)
            {
                distance += Double.Parse(route.Distance, numberFormatInfo);
                elevation += Double.Parse(route.Elevation, numberFormatInfo);
                time += TimeSpan.Parse(route.Time);
            }

            Statistics = new Statistics
            {
                RidesNumber = routes.Count,
                Distance = distance,
                Elevation = elevation,
                Time = time
            };
        }
    }
}