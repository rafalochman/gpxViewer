using gpxViewer.DataAccess;
using gpxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var routes = db.GpxRoutes.ToList();
            routes.Reverse();
            return View(routes);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(filePath);
                    GpxOperations gpxOperations = new GpxOperations();
                    gpxOperations.ReadGpx(filePath, fileName);
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

            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            var routes = db.GpxRoutes.ToList();
            routes.Reverse();
            return View(routes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}