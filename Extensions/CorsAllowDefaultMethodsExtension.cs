using Microsoft.AspNetCore.Cors.Infrastructure;

namespace TaxiDispacher.Extensions
{
    public static class CorsAllowDefaultMethodsExtension
    {
        private static string[] DEFAULT_METHODS = { "PUT", "POST", "DELETE", "GET"};
        public static CorsPolicyBuilder AllowDefaultMethodsExtension(this CorsPolicyBuilder builder)
        {
            builder.WithMethods(DEFAULT_METHODS);

            return builder;
        }
        public static CorsPolicyBuilder AllowDefaultMethodsExtension(this CorsPolicyBuilder builder, params string[] extraMethods)
        {
            var items = new List<string>(DEFAULT_METHODS);
            foreach (var method in extraMethods)
            {
                items.Add(method);
            }
            builder.WithMethods(items.ToArray());

            return builder;
        }
    }
}
