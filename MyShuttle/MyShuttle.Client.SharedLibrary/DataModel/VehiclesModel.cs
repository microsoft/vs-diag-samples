using Newtonsoft.Json;
using MyShuttle.Client.Core.DocumentResponse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyShuttle.Diagnostics.Service.Models
{
    public class VehiclesModel
    {
        static IEnumerable<Vehicle> m_vehicles;
        public static IEnumerable<Vehicle> Vehicles
        {
            get
            {
                if (m_vehicles == null)
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\vehicles.json";
                    string json;
                    using (StreamReader sr = new StreamReader(file))
                    {
                        json = sr.ReadToEnd();
                    }
                    m_vehicles = JsonConvert.DeserializeObject<IEnumerable<Vehicle>>(json);
                    m_vehicles = m_vehicles.OrderBy(v => v.DriverId);
                }
                return m_vehicles;
            }
        }

        static Dictionary<int, Driver> m_drivers;
        public static Dictionary<int, Driver> Drivers
        {
            get
            {
                if (m_drivers == null)
                {
                    string json;
                    string file = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\drivers.json";
                    using (StreamReader sr = new StreamReader(file))
                    {
                        json = sr.ReadToEnd();
                    }

                    m_drivers = JsonConvert.DeserializeObject<Dictionary<int, Driver>>(json);
                }
                return m_drivers;
            }
        }
    }
}