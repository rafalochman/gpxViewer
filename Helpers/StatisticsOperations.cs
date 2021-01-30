using gpxViewer.DataAccess;
using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpxViewer.Helpers
{
    public class StatisticsOperations
    {
        public Statistics statistics = new Statistics();
        
        public StatisticsOperations()
        {
            SetStatistics();
        }

        private void SetStatistics()
        {
            DefaultContext db = new DefaultContext();
            var routes = db.GpxRoutes.ToList();

            double distance = 0;
            double elevation = 0;
            TimeSpan time = new TimeSpan(0, 0, 0, 0, 0);

            foreach(GpxRoute route in routes)
            {
                distance += Double.Parse(route.Distance);
                elevation += Double.Parse(route.Elevation);
                time += TimeSpan.Parse(route.Time);
            }

            statistics.RidesNumber = routes.Count;
            statistics.Distance = distance;
            statistics.Elevation = elevation;
            statistics.Time = time;
        }
    }
}