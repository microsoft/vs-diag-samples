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
using System.Web.Http;

namespace ProjectArchive.Web.Controllers
{

    public class CertificatesController : ApiController
    {
        static Random rand = new Random();
        const int DelayTime = 600;
        const int Variance = 50;

        // GET: api/Certificates
        public string Get()
        {
            System.Threading.Thread.Sleep(rand.Next(DelayTime - Variance, DelayTime + Variance));
            return GetCertificateJson();
        }

        // GET: api/Certificates/5
        public string Get(string cert)
        {
            var certJson = StringUtilities.ConvertFromBase64(cert);
            var certificate = Newtonsoft.Json.JsonConvert.DeserializeObject<Certificate>(certJson);
            return GetCertificateJson();
        }

        private string GetCertificateJson()
        {
            var certJson = Certificate.GetNewCertificateJson();
            var base64cert = StringUtilities.ConvertToBase64(certJson);
            return base64cert;
        }

    }
}
