using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Diagnostics.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyShuttle.Client.Services;
using MyShuttle.Client.SharedLibrary.Cache;
using Newtonsoft.Json;
using System.Diagnostics;
using MyShuttle.Client.Web.Models;

public class HomeController : Controller
{
    private static string m_cachedResponse = null;
    private static DriverCache m_driverCache = new DriverCache();
    private static List<Driver> m_cachedDrivers = null;

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult About()
    {
        ViewBag.Message = "Your application description page.";

        return View();
    }

    public ActionResult Contact()
    {
        //GetAllDrivers();

        return View();
    }


    public List<Driver> GetAllDrivers()
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
        List<Driver> allDrivers;
        allDrivers = SortDriverListByName(TrimDriverListById(GetDriverList(), 30));
        return allDrivers;
    }

    private List<Driver> TrimDriverListById(List<Driver> allDrivers, int maxId)
    {
        List<Driver> trimmedDriverList = new List<Driver>();
        trimmedDriverList = allDrivers.Where(d => d.DriverId <= maxId && d.DriverId > 0).ToList();

        return trimmedDriverList;
    }

    private List<Driver> SortDriverListByName(List<Driver> allDrivers)
    {
        List<Driver> sortedDriverList = new List<Driver>();
        sortedDriverList = OrderList(allDrivers);
        return sortedDriverList;
    }

    private List<Driver> OrderList(List<Driver> listToOrder)
    {
        List<Driver> orderedDriverList = new List<Driver>();
        listToOrder.OrderBy(d => d.Name).ToList();
        return orderedDriverList;
    }

    public JsonResult AllDrivers()
    {
        Debug.WriteLine("Loading All Drivers...");
        var allDrivers = GetAllDrivers();
        var json = this.Json(allDrivers, JsonRequestBehavior.AllowGet);
        json.MaxJsonLength = int.MaxValue;
        SaveJpegImages(allDrivers);
        return json;
    }


    public JsonResult Vehicles()
    {
        Debug.WriteLine("Loading Vehicles...");
        var vehicles = VehiclesModel.Vehicles;

        var json = this.Json(vehicles, JsonRequestBehavior.AllowGet);
        json.MaxJsonLength = int.MaxValue;
        SaveJpegImages(vehicles);
        return json;
    }


    [System.Web.Mvc.HttpPost]
    public JsonResult Driver(string driverLookup)
    {
        Debug.WriteLine("Loading Drivers...");
        var driver = GetIndividualDriverCached(driverLookup);

        Driver clientDriver = null;

        Console.WriteLine("Getting driver ratings...");
        try
        {
            using (var db = new RatingsContext())
            {
                var results = from driverRating in db.DriverRatings
                              where driverRating.DriverId == driver.DriverId
                              select driverRating;

                double ratingAvg = results.First().RatingAvg;
                string ratingText = String.Format("{0:F1} out of 5", ratingAvg);

                //string ratingText = results.Count() > 0 ?
                //    String.Format("{0:F1} out of 5", results.First().RatingAvg) :
                //    "Unrated";

                clientDriver = new Driver()
                {
                    Name = driver.Name,
                    PictureUrl = driver.PictureUrl,
                    RatingText = ratingText
                };
            }
        }
        catch (Exception)
        {
            // Fail gracefully in case the database connection goes down
        }

        var json = Json(clientDriver, JsonRequestBehavior.AllowGet);
        return json;
    }

    private Driver GetDriverFromDriverList(List<Driver> drivers, DriverLookupRequest driver)
    {
        Debug.WriteLine("Getting Driver from Driver's List");

        Driver selectedDriver;

        try
        {
            selectedDriver =
            drivers.Where(d => driver.name.Equals(d.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
        catch (Exception)
        {
            // In case of failure return a placeholder driver record
            selectedDriver = new Driver()
            {
                DriverId = 0,
                Name = "Unknown",
                Picture = AppSettings.PlaceHolderPicture
            };
        }

        return selectedDriver;
    }

    private Driver GetIndividualDriver(string driverQuery)
    {
        var drivers = GetDriverList();
        var driverLookup = JsonConvert.DeserializeObject<DriverLookupRequest>(driverQuery);
        var driver = GetDriverFromDriverList(drivers, driverLookup);
        SaveJpegImage(driver);
        return driver;
    }

    private Driver GetIndividualDriverCached(string driverQuery)
    {

        var driver = m_driverCache.GetDriverFromCache(driverQuery);

        if (driver == null)
        {
            var drivers = GetDriverList(cacheResponse: true);
            var driverLookup = JsonConvert.DeserializeObject<DriverLookupRequest>(driverQuery);
            driver = GetDriverFromDriverList(drivers, driverLookup);
            m_driverCache.AddDriverToCache(driverQuery, driver);
            SaveJpegImage(driver);
        }
        return driver;
    }

    private List<Driver> GetDriverList(bool cacheResponse = false)
    {
        Debug.WriteLine("Getting Driver's List from Backend");

        // Use the cached response if one exist
        string driversStr = m_cachedResponse;
        if (driversStr == null)
        {
            var request = new BaseRequest(AppSettings.DriversWebApiUrl, string.Empty);
            driversStr = request.GetString(AppSettings.DriversWebApiUrl);

            // cache this response if the caller wants us to
            if (cacheResponse)
            {
                m_cachedResponse = driversStr;
                //m_cachedDrivers = DeserializeDriversJSON(driversStr);
            }
        }

        //return m_cachedDrivers;
        var drivers = DeserializeDriversJSON(driversStr);
        return drivers;
    }

    private List<Driver> DeserializeDriversJSON(string driversStr)
    {
        return JsonConvert.DeserializeObject<List<Driver>>(driversStr);
    }



    private void SaveJpegImage(Driver driver)
    {
        if (driver.Picture != null)
        {
            var pictureBytes = driver.Picture;
            driver.PictureUrl = "/Content/" + driver.Name + ".jpg";
            string path = System.Web.HttpContext.Current.Server.MapPath("~" + driver.PictureUrl);
            System.IO.File.WriteAllBytes(path, driver.Picture);
            driver.PictureContents = new byte[10 * 1024 * 1024];
        }
    }

    private void SaveJpegImages(IEnumerable<Vehicle> vehicles)
    {
        foreach (var vehicle in vehicles)
        {
            var pictureBytes = vehicle.Picture;
            vehicle.PictureUrl = "/Content/" + vehicle.Make + vehicle.Model + ".jpg";
            string path = System.Web.HttpContext.Current.Server.MapPath("~" + vehicle.PictureUrl);
            System.IO.File.WriteAllBytes(path, vehicle.Picture);
        }
    }

    private void SaveJpegImages(IEnumerable<Driver> allDrivers)
    {
        foreach (var driver in allDrivers)
        {
            var pictureBytes = driver.Picture;
            driver.PictureUrl = "/Content/" + driver.Name + ".jpg";
            string path = System.Web.HttpContext.Current.Server.MapPath("~" + driver.PictureUrl);
            System.IO.File.WriteAllBytes(path, driver.Picture);
        }
    }
}