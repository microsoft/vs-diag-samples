using MyShuttle.Client.Core.DocumentResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Desktop.Core.ServiceAgents.Cache
{
    public class WebRequestCache<T>
    {
    public static bool UseCache = false; //true;
        static string BinaryFileName { get { return "cache.bin"; } }
        static string TextFileExtension { get { return ".json"; } }

        private static Dictionary<string, T> m_cache;
        public static MemoryCacheChanged<T> CacheChanged;

        public static T[] AllRecords { get { return m_cache.Values.ToArray(); } }
        public static Task PreCacheDriversTask { get; set; }

        private static DiskCache FileCache
        {
            get
            {
                var dc = new DiskCache();
                CacheChanged += dc.WebRequestCache_FileChanged;
                return dc;
            }
        }

        public static void WriteCacheToFile(string fileName)
        {
            var itemsToWrite = new Dictionary<int, Driver>();
            foreach(var d in m_cache.Values)
            {
                Driver driver = d as Driver;
                if (driver.Picture == null)
                {
                    driver = GetFullItemFromDisk(driver.DriverId.ToString()) as Driver;
                }
                itemsToWrite.Add(driver.DriverId, driver);
            }
            string contents = JsonConvert.SerializeObject(itemsToWrite);
            var diskCache = FileCache;
            diskCache.writeTextFileToCache(fileName, contents, true);
            CacheChanged -= diskCache.WebRequestCache_FileChanged;
        }

        public static bool Expired(string key)
        {
            return false;
        }

        public static T GetItem(string key)
        {
            var fileCache = FileCache;
            T value;

            if (m_cache.ContainsKey(key))
            {
                value = m_cache[key];
            }
            else
            {
               value = GetFullItemFromDisk(key);
            }

            return value;
        }

        public static T GetFullItemFromDisk(string key)
        {
            var json = GetTextFromDisk(key);
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }

        public static bool ContainsKey(string key)
        {
            if (!UseCache)
            {
                return false;
            }
            if (WebRequestCache<Driver>.PreCacheDriversTask != null)
            {
                WebRequestCache<Driver>.PreCacheDriversTask.Wait();
            }

            return m_cache.ContainsKey(key);
        }

        public static void PreAdd(string key, T value)
        {
            if (!UseCache)
                return;

            //Write the contents to the disk cache since we don't want to hold the pictures in the in-memory cache;
            string fileToWrite = JsonConvert.SerializeObject(value);
            PersistTextToDisk(key, fileToWrite);

            // Some drivers have very large pictures.  Since we're prefetching we don't know if these  
            // will be used so don't cache the large images in memory, instead retrieve from disk
            // only if the record is accessed.
            if (value.GetType() == typeof(Driver))
            {
                Driver d = value as Driver;
                if (d.Picture.Length > 20480)
                {
                    d.Picture = null;
                }
            }

            //add to in memory cache
            m_cache[key] = value;
        }

        public static void Add(string key, T value)
        {
            if (!UseCache)
                return;

            //Write the contents to the disk cache since we don't want to hold the pictures in the in-memory cache;
            string fileToWrite = JsonConvert.SerializeObject(value);
            PersistTextToDisk(key, fileToWrite);

            //add to in memory cache
            m_cache[key] = value;
        }

        private static void PersistTextToDisk(string name, string data)
        {
            string fileName = getFileName(name);
            FileCache.writeTextFileToCache(fileName, data);
        }

        private static string GetTextFromDisk(string name)
        {
            string fileName = getFileName(name);
            return FileCache.readTextFileFromDiskCache(fileName);
        }

        private static string getFileName(string s)
        {
            return s + TextFileExtension;
        }

        private void NotifyOfChange()
        {
            CacheChangedEventArgs args = new CacheChangedEventArgs(BinaryFileName, CacheFileType.Binary);
            CacheChanged(m_cache, args);
        }

        static WebRequestCache()
        {
            if (UseCache)
            {
                object cache = null;// FileCache.readBinaryFileFromDiskCache(fileName);

                if (cache == null)
                {
                    m_cache = new Dictionary<string, T>();
                }
                else
                {
                    m_cache = cache as Dictionary<string, T>;
                }
            }

        }

        public static async Task ClearCache()
        {
            m_cache = new Dictionary<string, T>();
            await DiskCache.ClearCache();
        }
    }
}
