using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace gpxViewer.DataAccess
{
    public class GpxInitializer : DropCreateDatabaseIfModelChanges<GpxContext>
    {
        protected override void Seed(GpxContext context)
        {
            context.SaveChanges();
            base.Seed(context);
        }
    }
}