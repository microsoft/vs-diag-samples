using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyShuttle.Client.Web.Models
{
    public class DriverRating
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public double RatingAvg { get; set; }
    }

    public class RatingsContext : DbContext
    {
        public DbSet<DriverRating> DriverRatings { get; set; }
    }

    public class RatingsInitializer : DropCreateDatabaseIfModelChanges<RatingsContext>
    {
        protected override void Seed(RatingsContext context)
        {
            context.DriverRatings.Add(new DriverRating { DriverId = 4, RatingAvg = 3.2 });
            context.DriverRatings.Add(new DriverRating { DriverId = 2, RatingAvg = 4.5 });
            context.DriverRatings.Add(new DriverRating { DriverId = 3, RatingAvg = 5.3 });
            context.SaveChanges();
        }
    }
}