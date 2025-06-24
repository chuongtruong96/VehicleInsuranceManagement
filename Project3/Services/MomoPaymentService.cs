using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project3.Services
{
    public class MomoPaymentService : IPaymentService
    {
        private readonly HttpClient _client;

        public MomoPaymentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> InitiatePayment(double amount, string returnUrl, string notifyUrl)
        {
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMOBKUN20180529";
            string accessKey = "klm05TvNBzhg7h7j";
            string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
            string orderInfo = "Pay via MoMo";
            string orderId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string requestId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string requestType = "payWithATM";

            string rawHash = $"accessKey={accessKey}&amount={amount.ToString("F0")}&extraData=&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
            string signature;

            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
                signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            var data = new
            {
                partnerCode,
                partnerName = "Test",
                storeId = "MomoTestStore",
                requestId,
                amount = amount.ToString("F0"),
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                lang = "vi",
                extraData = "",
                requestType,
                signature
            };

            string jsonData = JsonConvert.SerializeObject(data);
            string result = await _client.PostAsync(endpoint, new StringContent(jsonData, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            if (jsonResult.ContainsKey("payUrl"))
            {
                return jsonResult["payUrl"];
            }

            throw new Exception("Failed to initiate payment with MoMo");
        }

        public async Task<bool> ConfirmPayment(string transactionId, string resultCode)
        {
            if (resultCode == "0")
            {
                // Save the payment data here
                return true;
            }
            return false;
        }
    }
}
