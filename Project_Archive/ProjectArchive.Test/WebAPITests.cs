using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ProjectArchive.Test
{
    [TestClass]
    public class WebAPITests
    {
        static string IISExpressUrl = "http://localhost:10531/";
        AppDataProvider dataProvider = new AppDataProvider(@"..\..\..\ProjectArchive.Web\App_Data\sampledata.json");

        [TestMethod]
        public void AllApps()
        {
            var apps = dataProvider.GetAllApps();

            Assert.IsTrue(apps.Count > 0);
        }

        [TestMethod]
        public void AppsByType()
        {
            var typesToTest = new string[] {"Web", "Mobile", "desktop" };
            var enc = new StringUtilities(EncodingFormats.Base64);
            var allApps = dataProvider.GetAllApps();

            foreach(var appType in typesToTest)
            {
                var encodedType = enc.EncodeString(appType);
                var apps = dataProvider.GetAppsByType(encodedType);
                var appsOfType = allApps.Where(a => a.AppType.Equals(appType, StringComparison.OrdinalIgnoreCase)).ToList();

                Assert.AreEqual(appsOfType.Count, apps.Count);
            }
        }
    }
}
