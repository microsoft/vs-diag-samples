using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.SharedLibrary;
using MyShuttle.Diagnostics.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyShuttle.Diagnostics.Service.Controllers
{
    public class DriversController : ApiController
    {
        public JsonResult<List<Driver>> Get()
        {
            //Thread.Sleep(2000);
            var drivers = VehiclesModel.Drivers.Values.ToList();
            for (int i = 0; i < 100; i++)
            {
                Driver a = new Driver();
                a.DriverId = Guid.NewGuid().GetHashCode();
                a.Name = Guid.NewGuid().ToString();
                drivers.Add(a);
            }

            var json = this.Json(drivers);
            return json;
        }
    }
}
