//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Certificate
{
    public Guid Identitiy { get; set; }
    public int ByteOffset { get; set; }

    public Certificate(Guid id, int offset)
    {
        this.Identitiy = id;
        this.ByteOffset = offset;
    }

    public static string GetNewCertificateJson()
    {
        Certificate cert = new Certificate(Guid.NewGuid(), 53);
        var certJson = JsonConvert.SerializeObject(cert);
        return certJson;
    }

    public static Certificate GenerateNewCertificate()
    {
        return new Certificate(Guid.NewGuid(), 53);
    }
}

public static class CertificateStore
{
    private static Dictionary<string, Certificate> Certificates { get; set; }

    static CertificateStore()
    {

    }
}
