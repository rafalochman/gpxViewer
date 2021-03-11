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
using System.Web.UI;
using gpxViewer.Services;

namespace gpxViewer.Controllers
{
    public class HomeController : Controller
    {
        private MapListService mapListService = new MapListService();
        private DefaultGpxDataService defaultGpxDataService = new DefaultGpxDataService();
        private readonly ILog Log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Main()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.maps = mapListService.GetMapList();
                ViewBag.key = config.BING_KEY;
                var gpxData = defaultGpxDataService.GetDefaultGpxData();
                return View(gpxData);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Main(HttpPostedFileBase file)
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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (GpxContext db = new GpxContext())
                {
                    if (db.UserAccounts.Any(user => user.Username == account.Username))
                    {
                        ViewBag.Message = "Username" + account.Username + " already exists";
                        return View();
                    }
                    db.UserAccounts.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Username + "successfully registered";
            }

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login login)
        {
            if (ModelState.IsValid)
            {
                using (GpxContext db = new GpxContext())
                {
                    var user = db.UserAccounts.SingleOrDefault(u =>
                        u.Username == login.Username && u.Password == login.Password);
                    if (user != null)
                    {
                        Session["UserId"] = user.UserId.ToString();
                        Session["Username"] = user.Username;
                        return RedirectToAction("Main", "Home");
                    }

                    ModelState.AddModelError("", "Username or Password is wrong");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }

}