using gpxViewer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace gpxViewer.DataAccess
{
    public class DefaultInitializer : DropCreateDatabaseIfModelChanges<DefaultContext>
    {
        protected override void Seed(DefaultContext context)
        {
            context.SaveChanges();
            base.Seed(context);
        }
    }
}