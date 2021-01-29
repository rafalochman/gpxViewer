using gpxViewer.DataAccess;
using gpxViewer.Helpers;
using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace gpxViewer.Controllers
{
    public class HomeController : Controller
    {
        private DefaultContext db = new DefaultContext();
        public ActionResult Index()
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            ViewBag.maps = maps;

            GpxData gpxData = new GpxData
            {
                Lat = new List<double> { 51.91 },
                Lng = new List<double> { 19.13 },
                Distances = new List<double> { 0.0 },
                Elevations = new List<double> { 0.0 },
                Distance = "",
                Elevation = "",
                Time = ""
            };

            return View(gpxData);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            ViewBag.maps = maps;
            GpxData gpxData = new GpxData();
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(filePath);
                    GpxOperations gpxOperations = new GpxOperations();
                    gpxOperations.ReadGpx(filePath, fileName);
                    gpxData = gpxOperations.gpxData;
                    TempData["Message"] = Resources.Resource.Sent;
                }
                catch (Exception e)
                {
                    TempData["Message"] = Resources.Resource.Error;
                }
            }
            else
            {
                TempData["Message"] = Resources.Resource.Error;
            }
            return View(gpxData);
        }
    }
}