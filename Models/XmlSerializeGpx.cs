using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace gpxViewer.Models
{
    public class XmlSerializeGpx
    {
        [XmlRoot(ElementName = "start")]
        public class Start
        {
            public double ele { get; set; }
            public DateTime time { get; set; }
            [XmlAttribute("lat")]
            public double lat { get; set; }
            [XmlAttribute("lon")]
            public double lon { get; set; }
        }

        [XmlRoot(ElementName = "trkpt")]
        public class Trkpt
        {
            public double ele { get; set; }
            public DateTime time { get; set; }
            [XmlAttribute("lat")]
            public double lat { get; set; }
            [XmlAttribute("lon")]
            public double lon { get; set; }
        }

        [XmlRoot(ElementName = "trkseg")]
        public class Trkseg
        {
            [XmlElement("start")]
            public List<Start> start { get; set; }
            [XmlElement("trkpt")]
            public List<Trkpt> trkpt { get; set; }
        }

        [XmlRoot(ElementName = "trk")]
        public class Trk
        {
            public Trkseg trkseg { get; set; }
        }

        [XmlRoot(ElementName = "gpx")]
        public class Gpx
        {
            public Trk trk { get; set; }
        }
    }
}