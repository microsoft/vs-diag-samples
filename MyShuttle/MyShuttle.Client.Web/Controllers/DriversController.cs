using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Services;
using MyShuttle.Client.SharedLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyShuttle.Client.Web.Controllers
{
    public class DriversController : ApiController
    {
        //const string SeconardyService = "http://myshuttleservice.azurewebsites.net/";
        const string SecondaryService = "http://myshuttlediagnosticsservice.azurewebsites.net/";
        //const string SeconardyService = "http://localhost:39012/";
        const string DriversWebApiUrl = "/api/Drivers";
        
        // GET: api/Drivers
        public JsonResult<List<Driver>> Get()
        {
            var url = SecondaryService + DriversWebApiUrl;
            var request = new BaseRequest(SecondaryService, string.Empty);
            var driversStr = request.GetString(url);
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(driversStr);

            // Save images to files and serve them up to the client
            SaveJpegImages(drivers);
            var json = this.Json(drivers);
            return json;
        }

        private void SaveJpegImages(List<Driver> drivers)
        {
            foreach (var driver in drivers)
            {
                var pictureBytes = driver.Picture;
                driver.PictureUrl = driver.Name + ".jpg";
                string path = HttpContext.Current.Server.MapPath("~/App_Data/" + driver.PictureUrl);
                Stream writer = new FileStream(path, FileMode.Create);
                writer.WriteAsync(driver.Picture, 0, driver.Picture.Length);
                writer.Close();
                driver.Picture = null;
            }
        }
        // GET: api/Drivers/5
        public string Get(int id)
        {
            return string.Empty;
        }
    }
}
