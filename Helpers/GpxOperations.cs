using gpxViewer.Configs;
using gpxViewer.DataAccess;
using gpxViewer.Models;
using PolylineEncoder.Net.Models;
using PolylineEncoder.Net.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace gpxViewer.Helpers
{
    public class GpxOperations
    {
        public GpxData GpxData { get; set; }
        public string Distance { get; set; }
        public string Elevation { get; set; }
        public string Time { get; set; }
        public string SentDate { get; set; }

        public GpxOperations(string filePath, string fileName)
        {
            ReadGpx(filePath, fileName);
        }

        private void ReadGpx(string filePath, string fileName)
        {
            List<double> lat = new List<double>();
            List<double> lng = new List<double>();
            List<double> elevations = new List<double>();
            List<DateTime> timeList = new List<DateTime>();

            NumberFormatInfo numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberDecimalSeparator = ".";

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlSerializeGpx.Gpx), "http://www.topografix.com/GPX/1/1");
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                XmlReader reader = XmlReader.Create(fileStream);
                XmlSerializeGpx.Gpx gpxObj = (XmlSerializeGpx.Gpx)xmlSerializer.Deserialize(reader);
                foreach (XmlSerializeGpx.Trkpt track in gpxObj.trk.trkseg.trkpt)
                {
                    lat.Add(track.lat);
                    lng.Add(track.lon);
                    elevations.Add(track.ele);
                    timeList.Add(track.time);
                }

                GpxData = new GpxData
                {
                    Lat = lat,
                    Lng = lng,
                    Distances = CalculateDistances(lat, lng),
                    Elevations = elevations
                };
                Distance = CalculateDistance(lat, lng).ToString("N", numberFormatInfo);
                Elevation = CalculateElevation(elevations).ToString("N", numberFormatInfo);
                Time = CalculateTime(timeList).ToString(@"hh\:mm\:ss");
                SentDate = DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("pl"));

                var context = new GpxContext();
                var route = new GpxRoute
                {
                    Name = fileName,
                    Distance = Distance,
                    Time = Time,
                    Elevation = Elevation,
                    SentDate = SentDate,
                    FilePath = filePath,
                    MapUrl = PrepareUrl(lat, lng)
                };
                if (!context.GpxRoutes.Any(r => r.Name == fileName) ||
                    !context.GpxRoutes.Any(r => r.Elevation == Elevation) ||
                    !context.GpxRoutes.Any(r => r.Time == Time) ||
                    !context.GpxRoutes.Any(r => r.Distance == Distance))
                {
                    context.GpxRoutes.Add(route);
                }
                context.SaveChanges();
                fileStream.Close();
            }
            catch (Exception e)
            {

            }
        }

        private string PrepareUrl(List<double> lat, List<double> lng)
        {
            var geoPoints = new List<IGeoCoordinate>();
            for (int i = 0; i < lat.Count; i++)
            {
                if (i % 50 == 0)
                {
                    geoPoints.Add(new GeoCoordinate(lat[i], lng[i]));
                }
            }

            var polylineUtility = new PolylineUtility();
            var polyLine = polylineUtility.Encode(geoPoints); 
            string encodedPolyline = HttpUtility.UrlEncode(polyLine);
            string url = "https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/path-3+FF0000(" + encodedPolyline + ")/auto/500x300?access_token=" + config.MAPBOX_KEY;
            return url;
        }
   

    private List<double> CalculateDistances(List<double> lat, List<double> lng)
        {
            List<double> distancesList = new List<double>();
            double distance = 0;
            for (int i = 0; i < lat.Count - 1; i++)
            {
                distance += ArcInMeters(lat[i], lng[i], lat[i + 1], lng[i + 1]);
                distancesList.Add(distance / 1000);
            }
            return distancesList;
        }

        private TimeSpan CalculateTime(List<DateTime> time)
        {
            var routeTime = time[time.Count - 1] - time[0];
            return routeTime;
        }

        private double CalculateElevation(List<double> ele)
        {
            double elevation = 0;
            for (int i = 0; i < ele.Count - 1; i++)
            {
                if ((ele[i + 1] - ele[i]) > 0.1)
                {
                    elevation += (ele[i + 1] - ele[i]);
                }
            }
            elevation = Math.Round(elevation, 2);
            return elevation;
        }

        private double CalculateDistance(List<double> lat, List<double> lng)
        {
            double distance = 0;
            for (int i = 0; i < lat.Count - 1; i++)
            {
                distance += ArcInMeters(lat[i], lng[i], lat[i + 1], lng[i + 1]);
            }
            distance = Math.Round(distance / 1000, 2);
            return distance;
        }

        private static double ArcInMeters(double lat0, double lon0, double lat1, double lon1)
        {
            double earthRadius = 6372797.560856;
            return earthRadius * ArcInRadians(lat0, lon0, lat1, lon1);
        }

        private static double ArcInRadians(double lat0, double lon0, double lat1, double lon1)
        {
            double latitudeArc = DegToRad(lat0 - lat1);
            double longitudeArc = DegToRad(lon0 - lon1);
            double latitudeH = Math.Sin(latitudeArc * 0.5);
            latitudeH *= latitudeH;
            double lontitudeH = Math.Sin(longitudeArc * 0.5);
            lontitudeH *= lontitudeH;
            double tmp = Math.Cos(DegToRad(lat0)) * Math.Cos(DegToRad(lat1));
            return 2.0 * Math.Asin(Math.Sqrt(latitudeH + tmp * lontitudeH));
        }

        private static double DegToRad(double x)
        {
            return x * Math.PI / 180;
        }
    }
}
