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
                key.Append(GenerateBase64String(options));
            }

            return key.ToString();
        }
        public static string GenerateBase64String(object obj)
        {
            var hash = Encoding.UTF8.GetBytes(obj.GetHashCode().ToString());
            return Convert.ToBase64String(hash);
        }
    }
}
