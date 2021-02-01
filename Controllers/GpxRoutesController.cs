using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gpxViewer.DataAccess;
using gpxViewer.Helpers;
using gpxViewer.Models;

namespace gpxViewer.Controllers
{
    public class GpxRoutesController : Controller
    {
        private GpxContext db = new GpxContext();

        // GET: GpxRoutes
        public ActionResult Index()
        {
            StatisticsOperations statisticsOperations = new StatisticsOperations();
            ViewBag.Rides = statisticsOperations.statistics.RidesNumber;
            ViewBag.Elevation = statisticsOperations.statistics.Elevation;
            ViewBag.Distance = statisticsOperations.statistics.Distance;
            ViewBag.Time = statisticsOperations.statistics.Time;
            var routes = db.GpxRoutes.ToList();
            routes.Reverse();
            return View(routes);
        }

        // GET: GpxRoutes/Details/5
        public ActionResult Details(int? id)
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            ViewBag.maps = maps;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GpxRoute gpxRoute = db.GpxRoutes.Find(id);
            ViewBag.id = gpxRoute.Id;
            ViewBag.distance = gpxRoute.Distance;
            ViewBag.Elevation = gpxRoute.Elevation;
            ViewBag.Time = gpxRoute.Time;
            ViewBag.sentDate = gpxRoute.SentDate;
            GpxOperations gpxOperations = new GpxOperations();
            gpxOperations.ReadGpx(gpxRoute.FilePath, gpxRoute.Name);
            GpxData gpxData = gpxOperations.gpxData;
            if (gpxRoute == null)
            {
                return HttpNotFound();
            }
            return View(gpxData);
        }

        // GET: GpxRoutes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GpxRoutes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Distance,Time,Elevation,SentDate,FilePath,MapUrl")] GpxRoute gpxRoute)
        {
            if (ModelState.IsValid)
            {
                db.GpxRoutes.Add(gpxRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gpxRoute);
        }

        // GET: GpxRoutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GpxRoute gpxRoute = db.GpxRoutes.Find(id);
            if (gpxRoute == null)
            {
                return HttpNotFound();
            }
            return View(gpxRoute);
        }

        // POST: GpxRoutes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Distance,Time,Elevation,SentDate,FilePath,MapUrl")] GpxRoute gpxRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gpxRoute).State = EntityState.Modified;
                db.Entry(gpxRoute).Property("FilePath").IsModified = false;
                db.Entry(gpxRoute).Property("MapUrl").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index", "GpxRoutes");
            }
            return View(gpxRoute);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GpxRoute gpxRoute = db.GpxRoutes.Find(id);
            if (gpxRoute == null)
            {
                return HttpNotFound();
            }
            return View(gpxRoute);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GpxRoute gpxRoute = db.GpxRoutes.Find(id);
            db.GpxRoutes.Remove(gpxRoute);
            db.SaveChanges();
            return RedirectToAction("Index", "GpxRoutes");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
