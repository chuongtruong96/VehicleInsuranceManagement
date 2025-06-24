using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System.Collections.Generic;

namespace Project3.Helpers
{
    public static class PayPalConfiguration
    {
        private static readonly IConfiguration Configuration;

        static PayPalConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>
            {
                { "mode", Configuration["PayPal:Mode"] },
                { "connectionTimeout", Configuration["PayPal:ConnectionTimeout"] },
                { "requestRetries", Configuration["PayPal:RequestRetries"] }
            };
        }

        private static string GetAccessToken()
        {
            string clientId = Configuration["PayPal:ClientId"];
            string clientSecret = Configuration["PayPal:ClientSecret"];
            return new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
        }

        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccessToken())
            {
                Config = GetConfig()
            };
            return apiContext;
        }
    }
}
