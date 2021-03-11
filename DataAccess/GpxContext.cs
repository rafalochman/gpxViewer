using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gpxViewer.DataAccess
{
    public class GpxContext : DbContext
    {
        public GpxContext() : base("GpxContext")
        {

        }
        public DbSet<GpxRoute> GpxRoutes { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}