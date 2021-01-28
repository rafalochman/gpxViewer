using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gpxViewer.DataAccess;
using gpxViewer.Models;

namespace gpxViewer.Controllers
{
    public class GpxRoutesController : Controller
    {
        private DefaultContext db = new DefaultContext();

        // GET: GpxRoutes
        public ActionResult Index()
        {
            return View(db.GpxRoutes.ToList());
        }

        // GET: GpxRoutes/Details/5
        public ActionResult Details(int? id)
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
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gpxRoute);
        }

        // GET: GpxRoutes/Delete/5
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

        // POST: GpxRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GpxRoute gpxRoute = db.GpxRoutes.Find(id);
            db.GpxRoutes.Remove(gpxRoute);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
