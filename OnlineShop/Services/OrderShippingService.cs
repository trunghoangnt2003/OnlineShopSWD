using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnlineShop.Interfaces;
using OnlineShop.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace OnlineShop.Services
{
    public class OrderShippingService : IOrderShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public OrderShippingService(IConfiguration config,HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }
        public async Task<string> CreateOrderShippingAsync(ShippingOrderModel request)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };
            var shippingOrderJson = JsonConvert.SerializeObject(request, settings);
            var content = new StringContent(shippingOrderJson, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create")
            {
                Headers =
                {
                    { "ShopId", _config["GHN:ShopID"] },
                    { "Token", _config["GHN:API"] },
                    
                },
                Content = content
            };

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);

                 return await response.Content.ReadAsStringAsync();


            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }


    }
}
