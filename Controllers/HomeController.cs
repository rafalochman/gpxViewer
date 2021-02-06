using gpxViewer.Configs;
using gpxViewer.DataAccess;
using gpxViewer.Helpers;
using gpxViewer.Models;
using log4net;
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
        private GpxContext db = new GpxContext();
        private DefaultGpxData defaultGpxData = new DefaultGpxData();
        private readonly ILog Log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            ViewBag.maps = maps;
            ViewBag.key = config.BING_KEY;
            var gpxData = defaultGpxData.GetDefaultGpxData();

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
            ViewBag.key = config.BING_KEY;
            var gpxData = new GpxData();
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(filePath);
                    GpxOperations gpxOperations = new GpxOperations(filePath, fileName);
                    gpxData = gpxOperations.GpxData;
                }
                catch (Exception e)
                {
                    gpxData = defaultGpxData.GetDefaultGpxData();
                    Log.Error(e.Message);
                }
            }
            else
            {
                gpxData = defaultGpxData.GetDefaultGpxData();
                Log.Error("File content length error");
            }

            if (gpxData.Lat.Count > 1)
            {
                TempData["Message"] = Resources.Resource.Sent;
            }
            else
            {
                TempData["Message"] = Resources.Resource.Error;
            }
            return View(gpxData);
        }
    }
}