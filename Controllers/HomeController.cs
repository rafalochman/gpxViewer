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

            var routes = db.GpxRoutes.ToList();
            routes.Reverse();
            return View(routes);
        }
    }
}