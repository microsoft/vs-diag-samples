using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsLibrary
{
    public class FileUtilities
    {
        const string RegistryKeyRoot = @"HKEY_CURRENT_USER\Software\ProjectArchive\";
        Dictionary<string, string> m_settings = new Dictionary<string, string>();

        public FileUtilities()
        {
            var settingKeys = Enumerable.Range(0, 100);
            var keys = new string[settingKeys.Count()];

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = "key" + i;
            }
            LoadSettings(keys);
        }


        //[System.Diagnostics.DebuggerNonUserCode]
        public void LoadSettings(string[] keys)
        {
            foreach (var key in keys)
            {
                try
                {
                    var val = 
                    GetKeyValue(RegistryKeyRoot, key);
                    m_settings.Add(RegistryKeyRoot + key, val);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }









        [System.Diagnostics.DebuggerNonUserCode]
        public string GetKeyValue(string key, string name)
        {
            var val = (string)Registry.GetValue(key, name, null);
            if(val == null)
            {
                throw new System.Exception("Something went wrong", new ArgumentException($"Key {key + name} has no value", new ArgumentException($"Registry Key {name} does not exist")));
            }
            return val;
        }

    }
}
