using System;
using System.Security.Cryptography;
using System.Text;

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
                key.Append(GenerateSHA256String(options));
            }

            return key.ToString();
        }
        public static string GenerateSHA256String(object obj)
        {
            var bytes = Encoding.UTF8.GetBytes(obj.GetHashCode().ToString());
            byte[] hash;

            using (var sha1 = new SHA1Managed())
            {
                hash = sha1.ComputeHash(bytes);
            }

            return string.Concat(Array.ConvertAll(hash, x => x.ToString("X2")));
        }
    }
}
