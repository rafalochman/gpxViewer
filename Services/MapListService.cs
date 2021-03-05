using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gpxViewer.Helpers;
using gpxViewer.Models;

namespace gpxViewer.Services
{
    public class MapListService
    {

        public List<SelectListItem> GetMapList()
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            return maps;
        }
    }
}