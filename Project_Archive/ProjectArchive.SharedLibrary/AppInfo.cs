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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppInfoContainer
{
    static Random rand = new Random();

    public List<AppInfo> Apps { get; set; }
    byte[] BinaryData { get; set; }

    public AppInfoContainer(List<AppInfo> apps)
    {
        this.Apps = apps;
        this.BinaryData = new byte[4096000];
    }

    public void CmbItemType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {

    }
}

public class AppInfo
{
    public int AppID { get; set; }
    public string AppName { get; set; }
    public string SourceLocation { get; set; }

    public string Description { get; set; }
    public string Comments { get; set; }

    public string AppType { get; set; }
    public OwnerInfo Owner { get; set; }
    public List<string> Languages { get; set; }
    public string DisplayLanguages
    {
        get
        {
            return GetLanguagesForDisplay();
        }
    }
    public string DataAccessTechnology { get; set; }
    public string PresentationTechnology { get; set; }

    public string GetLanguagesForDisplay()
    {
        StringBuilder sb = new StringBuilder();
        for (int x = 0; x < Languages.Count; x++)
        {
            sb.Append(Languages[x]);
            if (x < Languages.Count - 1)
            {
                sb.Append(", ");
            }
        }
        return sb.ToString();
    }
}

public class OwnerInfo
{
    public int OwnerID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
