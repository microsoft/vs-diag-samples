using MyShuttle.Diagnostics.Service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShuttle.Diagnostics.Service.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult Vehicles()
        {
            var json = this.Json(VehiclesModel.Vehicles, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult Driver(int id)
        {
            System.Threading.Thread.Sleep(SleepTime);
            var json = this.Json(VehiclesModel.Drivers[id], JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult AllDrivers(string ids)
        {
            var json = this.Json(VehiclesModel.Drivers.Values, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        static int SleepTime
        {
            get
            {
                var val = System.Web.Configuration.WebConfigurationManager.AppSettings["DriverSleepTime"];
                return int.Parse(val);
            }
        }
    }
}
