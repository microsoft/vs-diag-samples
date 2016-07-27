using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.SettingsLibrary
{
    public class FileUtilities
    {
        const string RegistryKeyRoot = @"HKEY_CURRENT_USER\Software\ProjectArchive\";
        Dictionary<string, string> m_settings = new Dictionary<string, string>();

        //  [DebuggerNonUserCode]
        public FileUtilities()
        {
            var settingKeys = Enumerable.Range(0, 15);
            var keys = new string[settingKeys.Count()];

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = "key" + i;
            }
            foreach (var key in keys)
            {
                try
                {
                    LoadSettings(key);
                }
                catch
                {
                    Debug.WriteLine("Couldn't load setting for " + key);
                }
            }
        }










        [DebuggerNonUserCode]
        public void LoadSettings(string key)
        {
            try
            {
                var val = GetKeyValue(RegistryKeyRoot, key);
                m_settings.Add(RegistryKeyRoot + key, val);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                throw new System.Collections.Generic.KeyNotFoundException("Key Not Found", ex);
            }
        }

        [DebuggerNonUserCode]
        public string GetKeyValue(string key, string name)
        {
            var val = (string)Registry.GetValue(key, name, null);
            if (val == null)
            {
                throw new ArgumentException($"Registry Key {key + name} does not exist");
            }
            return val;
        }
    }
}
