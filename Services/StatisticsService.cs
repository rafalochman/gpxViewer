using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using gpxViewer.DataAccess;
using gpxViewer.Models;

namespace gpxViewer.Services
{
    public class StatisticsService
    {
        public Statistics Statistics { get; set; }

        public StatisticsService()
        {
            SetStatistics();
        }

        private void SetStatistics()
        {
            var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberDecimalSeparator = ".";
            var db = new GpxContext();
            var routes = db.GpxRoutes.ToList();

            double distance = 0;
            double elevation = 0;
            var time = new TimeSpan(0, 0, 0, 0, 0);

            foreach (var route in routes)
            {
                distance += double.Parse(route.Distance, numberFormatInfo);
                elevation += double.Parse(route.Elevation, numberFormatInfo);
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