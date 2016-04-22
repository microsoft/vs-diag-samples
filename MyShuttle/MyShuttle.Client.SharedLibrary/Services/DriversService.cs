namespace MyShuttle.Client.Core.ServiceAgents
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyShuttle.Client.Core.DocumentResponse;
    using System.Globalization;
    using System.Text;
    using Services;
    using Services.Interfaces;

    public class DriversService : BaseRequest, IUpdatableUrl
    {
        public DriversService(string urlPrefix, string securityToken)
            : base(urlPrefix, securityToken)
        {

        }

        public async Task DeleteAsync(int driverId)
        {
            string url = String.Format(CultureInfo.InvariantCulture
              , "{0}drivers/Delete/{1}", _urlPrefix, driverId);

            await base.DeleteAsync(url);
        }

        public void CacheAllDrivers(int[] driverIds)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i<driverIds.Length; i++)
            {
                int id = driverIds[i];
                sb.Append(id);
                sb.Append(" ");
            }
            string url = String.Format(CultureInfo.InvariantCulture
                , "{0}api/Drivers", "http://localhost:27139/"/*_urlPrefix, sb.ToString()*/);

            base.PreCacheDrivers(url);
        }

        public Driver GetDriver(int driverId)
        {
            string url = String.Format(CultureInfo.InvariantCulture
                , "{0}Home/Driver?id={1}", _urlPrefix, driverId);

            return base.GetDriver(url, driverId);
        }

        public async Task<IEnumerable<Driver>> GetAsync()
        {
            string url = String.Format(CultureInfo.InvariantCulture
              , "{0}/Home/AllDrivers?ids=1", _urlPrefix);

            return await base.GetIEnumerableAsync<Driver>(url);
        }

        public new async Task<int> GetCountAsync(string filter)
        {
            string url = String.Format(CultureInfo.InvariantCulture
              , "{0}drivers/count?filter={1}", _urlPrefix, filter);

            return await base.GetAsync<int>(url);
        }

        public async Task<int> PostAsync(Driver driver)
        {
            string url = String.Format(CultureInfo.InvariantCulture
              , "{0}drivers/Post", _urlPrefix);

            return await base.PostAsync<int, Driver>(url, driver);
        }

        public async Task PutAsync(Driver driver)
        {
            string url = String.Format(CultureInfo.InvariantCulture
                , "{0}drivers/Put", _urlPrefix);

            await base.PutAsync<Driver>(url, driver);
        }
    }
}
