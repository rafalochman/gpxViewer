using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gpxViewer.DataAccess
{
    public class DefaultContext : DbContext
    {
        public DefaultContext() : base("DefaultContext")
        {

        }
        public DbSet<GpxRoute> GpxRoutes { get; set; }
    }
}