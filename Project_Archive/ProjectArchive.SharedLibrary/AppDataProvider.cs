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
using System.Diagnostics;
using System.Linq;

public class AppDataProvider
{
    const string DataRootPath = @"..\..\..\ProjectArchive.Web\App_Data\";
    static DataSource m_data;
    DataSource Data { get { return m_data; } }

    public AppDataProvider(string dataLocation)
    {
        m_data = new DataSource(dataLocation);
    }
    public AppDataProvider()
    {
        string dataLocation = @"..\..\..\ProjectArchive.Web\App_Data\";
        m_data = new DataSource(dataLocation);
    }

    public List<AppInfo> GetAppsByType(string encodedParameter)
    {
        var parameters = encodedParameter.Split(':');
        var format = StringUtilities.GetEncodingFromCode(parameters[0]);
        var encoder = new StringUtilities(format);
        string decodedType = encoder.DecodeString(parameters[1]);
        var apps = Data.Applications.Where(a => a.AppType.Equals(decodedType));
        return apps.ToList();
    }

    public List<AppInfo> GetAllApps()
    {
        return Data.Applications.ToList();
    }
}
    