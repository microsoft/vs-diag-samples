//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.IO;
using System.Net;
using System.Threading.Tasks;

public class WebUtilities
{
    public static async Task<string> Get(string url)
    {
        string responseStr = null;
        HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;

        using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
        {

            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());

            responseStr = reader.ReadToEnd();
        }

        return responseStr;
    }

    public static async Task<Certificate> GetCertificate(string url)
    {
        string responseStr = null;
        HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;

        using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());

            responseStr = reader.ReadToEnd();
        }

        var asciiResponse = StringUtilities.ConvertFromBase64(responseStr.Substring(1,responseStr.Length-2));
        var serverCert = Newtonsoft.Json.JsonConvert.DeserializeObject<Certificate>(asciiResponse);
        return serverCert;
    }
}
