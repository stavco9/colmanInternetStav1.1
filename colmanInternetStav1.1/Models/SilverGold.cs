using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace colmanInternetStav1._1.Models
{
    public static class SilverGold
    {
        private static Uri _apiUrl = new Uri("https://api.silvergoldbull.com/v1/products");
        private static string _apiKey = "aa957629c3de04ea8b184e00a135ad4d";
        private static string _apiHeader = "X-API-KEY";
        private static HttpClient client = new HttpClient();

        private static async Task<object> getJsonFromUrl(Uri url)
        {
            object lstProducts = new object();

            if (!client.DefaultRequestHeaders.Contains(_apiHeader))
                client.DefaultRequestHeaders.Add(_apiHeader, _apiKey);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string strResult = await response.Content.ReadAsStringAsync();

                lstProducts = JsonConvert.DeserializeObject<object>(strResult);
            }

            return lstProducts;
        }

        public static async Task<object> GetAllProducts()
        {
            return await getJsonFromUrl(_apiUrl);
        }

        public static async Task<object> GetProduct(string id)
        {
            Uri uriById = new Uri(_apiUrl.AbsoluteUri + "/" + id);

            return await getJsonFromUrl(uriById);
        }
    }
}
