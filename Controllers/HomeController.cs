using gpxViewer.Configs;
using gpxViewer.DataAccess;
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
using gpxViewer.Services;

namespace gpxViewer.Controllers
{
    public class HomeController : Controller
    {
        private GpxContext db = new GpxContext();
        private MapListService mapListService = new MapListService();
        private DefaultGpxDataService defaultGpxDataService = new DefaultGpxDataService();
        private readonly ILog Log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            ViewBag.maps = mapListService.GetMapList();
            ViewBag.key = config.BING_KEY;
            var gpxData = defaultGpxDataService.GetDefaultGpxData();

            return View(gpxData);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            ViewBag.maps = mapListService.GetMapList();
            ViewBag.key = config.BING_KEY;
            var gpxData = new GpxData();
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(filePath);
                    GpxOperationsService gpxOperationsService = new GpxOperationsService(filePath, fileName);
                    gpxData = gpxOperationsService.GpxData;
                }
                catch (Exception e)
                {
                    gpxData = defaultGpxDataService.GetDefaultGpxData();
                    Log.Error(e.Message);
                }
            }
            else
            {
                gpxData = defaultGpxDataService.GetDefaultGpxData();
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