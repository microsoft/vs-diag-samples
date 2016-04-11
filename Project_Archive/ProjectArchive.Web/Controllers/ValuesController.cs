//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ProjectArchive.Web.Controllers
{

    //[Authorize]
    public class ValuesController : ApiController
    {
        static AppDataProvider DataProvider = new AppDataProvider(AppDomain.CurrentDomain.BaseDirectory + @"App_Data\");

        // GET api/values
        public IEnumerable<AppInfo> Get()
        {
            var apps = DataProvider.GetAllApps();
            return apps;
        }

        // GET api/values/5
        public IEnumerable<AppInfo> Get(string encodedType)
        {
            var apps = DataProvider.GetAppsByType(encodedType);
            return apps;
        }

    }
}
