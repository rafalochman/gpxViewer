using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpxViewer.Models
{
    public class GpxRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Distance { get; set; }
        public string Time { get; set; }
        public string Elevation { get; set; }
        public string SentDate { get; set; }
        public string FilePath { get; set; }
        public string MapUrl { get; set; }
    }
}