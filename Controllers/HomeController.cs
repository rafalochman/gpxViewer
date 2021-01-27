using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gpxViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SelectListItem> maps = new List<SelectListItem>() {
                new SelectListItem{Text="Bing Map", Value = "1"},
                new SelectListItem{Text="Google Map", Value = "2"},
                new SelectListItem{Text="Open Street Map", Value = "3"}
            };
            ViewBag.maps = maps;
            return View();
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