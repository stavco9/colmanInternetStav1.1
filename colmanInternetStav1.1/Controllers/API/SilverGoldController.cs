using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using colmanInternetStav1._1.Models;
using Newtonsoft.Json;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/SilverGold")]
    public class SilverGoldController : Controller
    {
        private int getRandomNumber(int length)
        {
            Random rnd = new Random();

            return rnd.Next(0, length);
        }

        [HttpGet("{id}")]
        public object getProductById(string id)
        {
            return null;
        }

        [HttpGet]
        public async Task<Dictionary<string, object>> GetOneProduct()
        {
            object products = await SilverGold.GetAllProducts();

            List<Dictionary<string, object>> lstProducts = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(products.ToString());

            int randObject = getRandomNumber(lstProducts.Count);
            
            object selectedProduct = await SilverGold.GetProduct(lstProducts[randObject]["id"].ToString());

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(selectedProduct.ToString());
        }
    }
}