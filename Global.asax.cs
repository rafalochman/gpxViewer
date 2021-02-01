using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace gpxViewer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
        }
    }
}
