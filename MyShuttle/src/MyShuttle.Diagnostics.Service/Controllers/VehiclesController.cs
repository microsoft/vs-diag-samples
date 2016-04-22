using MyShuttle.Diagnostics.Service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MyShuttle.Diagnostics.Service.Controllers
{
    public class VehiclesController : ApiController
    {
        // GET api/<controller>
        public int Get(/*int id*/)
        {
            return VehiclesModel.Vehicles.Count();
        }

    }
}