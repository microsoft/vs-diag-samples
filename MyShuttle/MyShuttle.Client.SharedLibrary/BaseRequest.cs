using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using MyShuttle.Client.Desktop.Core.ServiceAgents.Cache;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.SharedLibrary;
using System.Diagnostics;

namespace MyShuttle.Client.Services
{
    public interface IBaseRequest
    {
        /// <summary>
        /// Refreshes the security token.
        /// </summary>
        /// <param name="securityToken"></param>
        void RefreshToken(string securityToken);
    }

    public class BaseRequest : IBaseRequest
    {
        protected string _urlPrefix = string.Empty;
        protected string _securityToken = string.Empty;

        public string UrlPrefix
        {
            get
            {
                return _urlPrefix;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) ||
                    !Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    return;
                }

                _urlPrefix = value;
            }
        }

        public BaseRequest(string urlPrefix, string securityToken)
        {
            if (String.IsNullOrEmpty(urlPrefix))
                throw new ArgumentNullException("urlPrefix");

            if (!urlPrefix.EndsWith("/"))
                urlPrefix = string.Concat(urlPrefix, "/");

            _urlPrefix = urlPrefix;
            _securityToken = securityToken.StartsWith("Bearer ") ? securityToken.Substring(7) : securityToken;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _securityToken);
            return httpClient;
        }

        string LocalUrl = "http://localhost:39012/api/Vehicles";

        protected async Task<int> GetCountAsync(string url)
        {
            HttpClient httpClient = CreateHttpClient();
            var result = await httpClient.GetStringAsync(url);
            int count = int.Parse(result);
            return count;
        }

        protected void PreCacheDrivers(string url)
        {
            if (!WebRequestCache<Driver>.UseCache)
            {
                return;
            }

            WebRequestCache<Driver>.PreCacheDriversTask = Task.Run(() =>
            {

                HttpClient httpClient = CreateHttpClient();
                Driver[] allDrivers;

                try
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    allDrivers = JsonConvert.DeserializeObject<Driver[]>(response);

                    foreach (Driver driver in allDrivers)
                    {
                        WebRequestCache<Driver>.PreAdd(driver.DriverId.ToString(), driver);
                    }

                }
                catch (Exception e)
                {
                    //ignore for now, this was just an attempt to get them early
                    Debug.WriteLine(e);
                }
            });
        }

        protected Driver GetDriver(string url, int id)
        {
            Driver result;
            string key = id.ToString();

            if (!WebRequestCache<Driver>.ContainsKey(key) || WebRequestCache<Driver>.Expired(key) || !WebRequestCache<Driver>.UseCache)
            {
                HttpClient httpClient = CreateHttpClient();

                try
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    result = JsonConvert.DeserializeObject<Driver>(response);
                    WebRequestCache<Driver>.Add(key, result);
                }
                catch (Exception)
                {
                    result = new Driver();
                }
            }
            else
            {
                result = WebRequestCache<Driver>.GetItem(key);
                ////Uncomment to fix pre-cache bug
                //if (result.Picture == null)
                //{
                //  result = WebRequestCache<Driver>.GetFullItemFromDisk(key);
                //}
            }

            return result;
        }

        /// <summary>
        /// Do GetByVisitor
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<T> GetAsync<T>(string url)
            where T : new()
        {
            T result;
            if (!WebRequestCache<T>.ContainsKey(url) || WebRequestCache<T>.Expired(url))
            {
                HttpClient httpClient = CreateHttpClient();

                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    result = JsonConvert.DeserializeObject<T>(response);
                    WebRequestCache<T>.Add(url, result);
                }
                catch (Exception)
                {
                    result = new T();
                }
            }
            else
            {
                string jsonResponse = WebRequestCache<T>.GetItem(url) as string;
                result = JsonConvert.DeserializeObject<T>(jsonResponse);
            }

            return result;
        }

        protected void writeStringToFile(string towrite, string name)
        {
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(name))
            {
                sr.Write(towrite);
            }
        }

        protected string readStringFromFile(string name)
        {
            string toReturn;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(name))
            {
                toReturn = sr.ReadToEnd();
            }
            return toReturn;
        }

        protected async Task<IEnumerable<T>> GetIEnumerableAsync<T>(string url)
        {
            HttpClient httpClient = CreateHttpClient();
            IEnumerable<T> result;

            try
            {
                string actualUrl = LocalUrl;
                var response = await httpClient.GetStringAsync(url);
                result = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(response));
            }
            catch (Exception e)
            {
                result = Enumerable.Empty<T>();
            }

            return result;
        }

        /// <summary>
        /// Do GetByVisitor
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetString(string url)
        {
            HttpClient httpClient = CreateHttpClient();
            var str = httpClient.GetStringAsync(url).Result;
            return str;
        }

        /// <summary>
        /// Do post with results
        /// </summary>
        /// <param name="url"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, U>(string url, U entity)
        {
            HttpClient httpClient = CreateHttpClient();

            var content = JsonConvert.SerializeObject(entity);
            var response = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            string responseContent = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(responseContent));
        }

        /// <summary>
        /// Do post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostAsync<T>(string url, T entity)
        {
            HttpClient httpClient = CreateHttpClient();

            var content = JsonConvert.SerializeObject(entity);
            var response = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Do post
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task PostAsync(string url)
        {
            HttpClient httpClient = CreateHttpClient();

            var response = await httpClient.PostAsync(url, null);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="url"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutAsync<T>(string url, T entity)
        {
            HttpClient httpClient = CreateHttpClient();

            var content = JsonConvert.SerializeObject(entity);
            var response = await httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string url)
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.DeleteAsync(url);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Refreshes the security token.
        /// </summary>
        /// <param name="securityToken"></param>
        public void RefreshToken(string securityToken)
        {
            _securityToken = securityToken;
        }
    }
}
