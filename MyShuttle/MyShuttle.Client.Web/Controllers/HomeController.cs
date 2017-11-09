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

public class HomeController : Controller
{
    private static string m_cachedResponse = null;
    private static DriverCache m_driverCache = new DriverCache();
    private static List<Driver> m_cachedDrivers = null;
    private static int m_ratingThreshold = 10;
    public DriverManager m_driverManager = null;

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

    public List<Driver> GetBestDrivers()
    {
        this.m_driverManager = new DriverManager(GetDriverList(cacheResponse: true));
        m_driverManager.TrimDriversWithLowRatings(m_ratingThreshold);
        m_driverManager.SortDriversByRating();
        return m_driverManager.BestDrivers;
    }

    private List<Driver> TrimDriverListById(List<Driver> allDrivers, int maxId)
    {

        List<Driver> trimmedDriverList = initializeList();

        foreach (Driver driver in allDrivers)
        {
            if (driver.DriverId <= maxId && driver.DriverId > 0)
            {
                trimmedDriverList.Add(driver);
            }
        }

        return trimmedDriverList;
    }


    private List<Driver> initializeList()
    {
        //ToDo setup code to initalize list to return new List<Driver>()
        //return null;
        return new List<Driver>();
    }

    private List<Driver> SortDriverListBySettings(List<Driver> allDrivers)
    {
        List<Driver> sortedDriverList = initializeList();
        sortedDriverList = OrderList(allDrivers);
        return sortedDriverList;
    }

    private List<Driver> OrderList(List<Driver> listToOrder)
    {
        List<Driver> orderedDriverList = initializeList();
        orderedDriverList = listToOrder.OrderBy(d => d.Name).ToList();
        return orderedDriverList;
    }

    private int getMaxId()
    {
        return 10;
    }

    public JsonResult AllDrivers()
    {
        Debug.WriteLine("Loading All Drivers...");
        var allDrivers = GetBestDrivers();
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
        foreach (var vehicle in vehicles)
        {
            SaveJpegImage(vehicle);
        }
        
        return json;
    }


    [System.Web.Mvc.HttpPost]
    public JsonResult Driver(string driverLookup)
    {
        Debug.WriteLine("Loading Drivers...");
        var driver = GetIndividualDriverCached(driverLookup);
        var clientDriver = new Driver()
        {
            Name = driver.Name,
            PictureUrl = driver.PictureUrl
        };
        var json = Json(clientDriver, JsonRequestBehavior.AllowGet);
        return json;
    }

    private Driver GetDriverFromDriverList(List<Driver> drivers, DriverLookupRequest driver)
    {
        Debug.WriteLine("Getting Driver from Driver's List");

        Driver selectedDriver;

        try
        {
            //selectedDriver = drivers.Where(d => driver.id.Equals(d.DriverId)).FirstOrDefault();
            //this causes an exception. 
            selectedDriver = drivers.Where(d => driver.name.Equals(d.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

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
            //selectedDriver = new Driver();
            //selectedDriver.DriverId = 0;
            //selectedDriver.Name = "Unknown";
            //selectedDriver.Picture = AppSettings.PlaceHolderPicture;

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
        //Debug.WriteLine("Getting Driver's List from Backend");

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
                m_cachedDrivers = JsonConvert.DeserializeObject<List<Driver>>(driversStr);
            }
            return m_cachedDrivers;
        }

        //return m_cachedDrivers;
        var drivers = JsonConvert.DeserializeObject<List<Driver>>(driversStr);
        return drivers;
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

    private void SaveJpegImage(Vehicle vehicle)
    {
        
            var pictureBytes = vehicle.Picture;
            vehicle.PictureUrl = "/Content/" + vehicle.Make + vehicle.Model + ".jpg";
            string path = System.Web.HttpContext.Current.Server.MapPath("~" + vehicle.PictureUrl);
            System.IO.File.WriteAllBytes(path, vehicle.Picture);
       
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


public class DriverManager
{

    public List<Driver> BestDrivers { get; set; }
    public List<Vehicle> Vehicles { get; }

    // Constructor that takes one argument.
    public DriverManager(List<Driver> drivers)
    {
        BestDrivers = drivers;
    }

    public DriverManager(List<Driver> drivers, List<Vehicle> vehicles)
    {
        BestDrivers = drivers;
        Vehicles = vehicles;
    }

    public void TrimDriversWithLowRatings(int ratingThreshold)
    {
        this.BestDrivers.RemoveAll(driver => driver.RatingAvg < ratingThreshold);
    }

    public void SortDriversByRating()
    {
        this.BestDrivers.OrderBy(d => d.RatingAvg).ToList();
    }

    public string getDriverRatingDescription(Driver driver)
    {
        var description = "";
        if (driver.RatingAvg >= 0 && driver.RatingAvg <= 2.5)
            description = "Low";
        else if (driver.RatingAvg > 2.5 && driver.RatingAvg <= 4.5)
            description = "Medium";
        else if (driver.RatingAvg >= 4.5)
            description = "High";
        return description;
    }
}
