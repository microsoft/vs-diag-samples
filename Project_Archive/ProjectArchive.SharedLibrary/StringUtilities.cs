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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public enum EncodingFormats { ASCII, Base64 }

public class StringUtilities
{
    public EncodingFormats EncodingFormat { get; set; }

    public static string ASCIIFormatCode { get { return "0"; } }
    public static string Base64FormatCode { get { return "1"; } }

    public static string GetFormatCode(EncodingFormats format)
    {
        if (format == EncodingFormats.ASCII)
        {
            return ASCIIFormatCode;
        }

        return Base64FormatCode;
    }

    public static EncodingFormats GetEncodingFromCode(string code)
    {
        if (code == ASCIIFormatCode)
        {
            return EncodingFormats.ASCII;
        }

        return EncodingFormats.Base64;
    }

    public StringUtilities(EncodingFormats format)
    {
        EncodingFormat = format;
    }

    public static string ReverseString(string str)
    {
        int length = str.Length;
        if (length > 1)
        {
            var first = str.Substring(1);
            var last = str.Substring(0, 1);
            var revStr = ReverseString(first) + last;
            return revStr;
        }
        
        return str;
    }

    public static string ToXML(string json)
    {
        List<AppInfo> subReq = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AppInfo>>(json);
        XmlSerializer xsSubmit = new XmlSerializer(typeof(List<AppInfo>));
        StringWriter sww = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sww);
        xsSubmit.Serialize(writer, subReq);
        var xml = sww.ToString();
        return xml;
    }

    public string DecodeString(string encodedString)
    {
        var encoder = new StringEncoder(this.EncodingFormat);
        Debug.WriteLine($"Decoding {encodedString}");
        return encoder.DecodeString(encodedString);
    }

    public string EncodeString(string plainString, Certificate cert)
    {
        var encoder = new StringEncoder(this.EncodingFormat);
        Debug.WriteLine($"Encoding {plainString}");
        return GetFormatCode(this.EncodingFormat) + ":" + encoder.EncodeString(plainString);
    }

    public static string ConvertToBase64(string original)
    {
        var bytes = Encoding.ASCII.GetBytes(original);
        var base64String = StringEncoder.ConvertBytesToString(bytes, EncodingFormats.Base64);
        return base64String;
    }

    public static string ConvertFromBase64(string base64)
    {
        var bytes = StringEncoder.GetBytesFromString(base64, EncodingFormats.Base64);
        var asciiString = StringEncoder.ConvertBytesToString(bytes, EncodingFormats.ASCII);
        return asciiString;
    }

    public static List<AppInfo> DeserializeApps(string json)
    {
        //for (int i = 0; i < 200000; i++)
        //{
        //    var r = new Random();
        //}
        return JsonConvert.DeserializeObject<List<AppInfo>>(json);
    }
}

class StringEncoder
{
    //public bool UseBase64Encoding { get; set; }
    public EncodingFormats EncodingFormat { get; set; }

    byte m_byteShift;

    public StringEncoder(EncodingFormats format)
    {
        this.EncodingFormat = format;
        m_byteShift = 53;
    }

    public string EncodeString(string original)
    {
        byte[] originalBytes = Encoding.ASCII.GetBytes(original);
        byte[] encodedBytes = encodeBytes(originalBytes);
        string encodedString = ConvertBytesToString(encodedBytes, this.EncodingFormat);

        return encodedString;
    }

    public string DecodeString(string original)
    {
        byte[] originalBytes = GetBytesFromString(original, this.EncodingFormat);
        byte[] decodedBytes = decodeToBytes(originalBytes);
        string decodedString = Encoding.ASCII.GetString(decodedBytes);

        return decodedString;
    }

    public static byte[] GetBytesFromString(string original, EncodingFormats format)
    {
        if (format == EncodingFormats.Base64)
        {
            return Convert.FromBase64String(original);
        }
        else
        {
            return Encoding.ASCII.GetBytes(original);
        }
    }

    public static string ConvertBytesToString(byte[] encodedBytes, EncodingFormats format)
    {
        if (format == EncodingFormats.Base64)
        {
            return Convert.ToBase64String(encodedBytes);
        }
        else
        {
            return Encoding.ASCII.GetString(encodedBytes);
        }
    }

    byte[] encodeBytes(byte[] bytesToEncode)
    {
        byte[] encodedBytes = new byte[bytesToEncode.Length];

        for (int x = 0; x < bytesToEncode.Length; x++)
        {
            encodedBytes[x] = applyByteShift(bytesToEncode[x]);
        }

        return encodedBytes;
    }

    string encodeBytesToString(byte[] bytesToEncode)
    {
        byte[] encodedBytes = new byte[bytesToEncode.Length];

        for (int x = 0; x < bytesToEncode.Length; x++)
        {
            encodedBytes[x] = applyByteShift(bytesToEncode[x]);
        }

        return Encoding.ASCII.GetString(encodedBytes);
    }

    byte[] decodeToBytes(byte[] encodedBytes)
    {
        byte[] decodedBytes = new byte[encodedBytes.Length];

        for (int x = 0; x < encodedBytes.Length; x++)
        {
            decodedBytes[x] = removeByteShift(encodedBytes[x]);
        }

        return decodedBytes;
    }

    byte applyByteShift(byte byteToMask)
    {
        int masked = (byteToMask + m_byteShift);
        masked = masked % 127;
        return (byte)(masked);
    }

    byte removeByteShift(byte byteToUnMask)
    {
        int unmasked = (byteToUnMask - m_byteShift);
        if (unmasked < 0)
        {
            unmasked += 127;
        }
        return (byte)unmasked;
    }
}

