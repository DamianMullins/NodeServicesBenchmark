using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace NodeServicesBenchmark.Website.Extensions
{
    public static class TemplateServiceExtensions
    {
        public static string BuildTemplateKey(this string moduleName, object options = null)
        {
            if (string.IsNullOrWhiteSpace(moduleName)) throw new ArgumentException(nameof(moduleName));

            var key = new StringBuilder();

            key.Append("TEMPLATE:");
            key.Append(moduleName);

            if (options != null)
            {
                key.Append(GenerateHashedKey(options));
            }

            return key.ToString();
        }

        private static string GenerateHashedKey(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return GetSha256FromString(json);
        }

        private static string GetSha256FromString(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData);
            var hashString = new SHA256Managed();
            var hashValue = hashString.ComputeHash(message);
            var hex = new StringBuilder();

            foreach (var x in hashValue)
            {
                hex.AppendFormat("{0:x2}", x);
            }

            return hex.ToString();
        }
    }
}
