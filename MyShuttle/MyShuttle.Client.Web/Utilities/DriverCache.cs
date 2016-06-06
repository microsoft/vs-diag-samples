using MyShuttle.Client.Core.DocumentResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShuttle.Client.SharedLibrary.Cache;

public class DriverCache
{
    private Dictionary<string, Driver> m_cache = new Dictionary<string, Driver>();

    public void AddDriverToCache(string key, Driver driver)
    {
        //var driverInfo = JsonConvert.DeserializeObject<DriverLookupRequest>(key);
        //key = driverInfo.id.ToString();

        m_cache[key] = driver;
    }

    public Driver GetDriverFromCache(string key)
    {
        //var driverInfo = JsonConvert.DeserializeObject<DriverLookupRequest>(key);
        //key = driverInfo.id.ToString();

        Driver driver = null;
        m_cache.TryGetValue(key, out driver);
        return driver;
    }
}
