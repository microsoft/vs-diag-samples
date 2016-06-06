using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Desktop.Core.ServiceAgents.Cache
{
    class DiskCache
    {
        private static string CacheFolderName = "FileCache";
        //Full directory path of the disk cache
        private static string CacheDirectory { get; set; }
        private static string m_cachePath;
        private static DirectoryInfo m_cacheDir;
        private byte[] m_memPadding;

        /// <summary>
        /// Static constructor creates the cache folder if it doesn't exist. 
        /// Doing it in a static constructor prevents the need to check everytime the cache is accessed
        /// the only time this would be a problem is if the cache directory was deleted
        /// </summary>
        static DiskCache()
        {
            m_cachePath =  Directory.GetCurrentDirectory() + "\\" + CacheFolderName;
            if (!Directory.Exists(m_cachePath))
            {
                m_cacheDir = Directory.CreateDirectory(m_cachePath);
            }
            else
            {
                m_cacheDir = new DirectoryInfo(m_cachePath);
            }
            
            CacheDirectory = m_cachePath + "\\";
        }

        public async static Task ClearCache()
        {
            await Task.Run(() =>
            {
                foreach (var file in m_cacheDir.GetFiles())
                {
                    file.Delete();
                }
            });
        }

        public DiskCache()
        {
            m_memPadding = new byte[5000000];
        }

        public void WebRequestCache_FileChanged(object sender, CacheChangedEventArgs e)
        {
            //TO DO: We should move the implementation to update the cache based on a change event rather than relying on an explicit call.
        }


        /// <summary>
        /// Write an ASCII text file to the cache
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="text">the contents of the file</param>
        public void writeTextFileToCache(string fileName, string text, bool fullPathProvided = false)
        {
            string file;
            if (!fullPathProvided)
            {
                file = CacheDirectory + fileName;
            }
            else
            {
                file = fileName;
            }

            StreamWriter writer = new StreamWriter(file, false);
            writer.Write(text);
            writer.Dispose();
        }

        /// <summary>
        /// Write a binary file to the disk cache
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="dataToCache">the contents of the file</param>
        public void writeBinaryFileToDiskCache(string fileName, object dataToCache)
        {
            string fullFileName = CacheDirectory + fileName;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fullFileName,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, dataToCache);
            stream.Dispose();
        }

        /// <summary>
        /// Reads a binary file from the disk cache
        /// </summary>
        /// <param name="fileName">Name of the file to read</param>
        /// <returns>The contents of the binary file</returns>
        public object readBinaryFileFromDiskCache(string fileName)
        {
            object persistedFile = null;
            string fullFileName = CacheDirectory + fileName;
            if (File.Exists(fullFileName))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fullFileName,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                try
                {
                    persistedFile = formatter.Deserialize(stream);
                }
                catch
                {
                    /*there was an error reading it*/
                }
                finally
                {
                    stream.Dispose();
                }
            }
            return persistedFile;
        }

        /// <summary>
        /// Reads an ASCII text file from the disk cache
        /// </summary>
        /// <param name="fileName">File name to read from the cache</param>
        /// <returns>The contents of the file</returns>
        public string readTextFileFromDiskCache(string fileName)
        {
            string file = CacheDirectory + fileName;
            string text = null;
            if (File.Exists(file))
            {
                StreamReader stream = new StreamReader(file);
                text = stream.ReadToEnd();
                stream.Dispose();
            }
            return text;
        }
    }

    public enum CacheFileType { Text, Binary }

    public class CacheChangedEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public CacheFileType FileType { get; set; }

        public CacheChangedEventArgs(string fileName, CacheFileType fileType)
        {
            this.FileName = fileName;
            this.FileType = fileType;
        }
    }

    public delegate void MemoryCacheChanged<T>(Dictionary<string, T> cache, CacheChangedEventArgs args);
}
