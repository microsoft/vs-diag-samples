using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.SharedLibrary
{
    public enum EncodingFormats { ASCII, Base64 }

    public class Utilities
    {
        public static string ConvertToBase64(string original)
        {
            var bytes = Encoding.ASCII.GetBytes(original);
            var base64String = ConvertBytesToString(bytes, EncodingFormats.Base64);
            return base64String;
        }

        public static string ConvertFromBase64(string base64)
        {
            var bytes = GetBytesFromString(base64, EncodingFormats.Base64);
            var asciiString = ConvertBytesToString(bytes, EncodingFormats.ASCII);
            return asciiString;
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
    }
}
