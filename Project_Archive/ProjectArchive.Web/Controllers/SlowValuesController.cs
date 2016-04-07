//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.Collections.Generic;
using System.Web.Http;

namespace ProjectArchive.Web.Controllers
{
    public class SlowValuesController : ApiController
    {
        const int WaitTime = 1500;
        ValuesController values = new ValuesController();

        public IEnumerable<AppInfo> Get()
        {
            System.Threading.Thread.Sleep(WaitTime);
            return values.Get();
        }

        // GET api/values/5
        public IEnumerable<AppInfo> Get(string encodedType)
        {
            System.Threading.Thread.Sleep(WaitTime);
            return values.Get(encodedType);
        }

    }
}
