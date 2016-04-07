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
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;

public class DataSource
{
    public string IndexHtml { get; private set; }
    public string ApplicationJson { get; private set; }
    public string ApplicationXml { get; private set; }
    public List<AppInfo> Applications { get; private set; }
    public DataSet ApplicationDataSet { get; private set; }
    public bool Succeeded { get; set; }


    public DataSource(string dataRoot)
    {
        var jsonFile = dataRoot + "sampledata.json";
        var xmlFile = dataRoot + "sampledata.xml";
        var htmlFile = dataRoot + "Index.html";

        var reader = System.IO.File.OpenText(jsonFile);
        var json = reader.ReadToEnd();
        reader.Dispose();

        this.ApplicationJson = new StreamReader(jsonFile).ReadToEnd(); 
        this.Applications = JsonConvert.DeserializeObject<List<AppInfo>>(json);
        this.IndexHtml = new StreamReader(htmlFile).ReadToEnd();
        this.ApplicationXml = new StreamReader(xmlFile).ReadToEnd();
        this.ApplicationDataSet = new DataSet("Apps");
        this.ApplicationDataSet.ReadXml(xmlFile); 
    }

}