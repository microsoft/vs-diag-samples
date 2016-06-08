using MyShuttle.Diagnostics.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyShuttle.Client.Web.Controllers
{
    //this should hopefully give it a different checksum
    public class VehiclesController : ApiController
    {
        public int Get(/*int id*/)
        {
            return VehiclesModel.Vehicles.Count();
        }
    }
}
