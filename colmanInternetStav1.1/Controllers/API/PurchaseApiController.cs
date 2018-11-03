using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using colmanInternetStav1._1.Models;
using System.Net;
using System.IO;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Purchase")]
    public class PurchaseApiController : Controller
    {
        [HttpPut]
        public async Task<IActionResult> MakePurchase([FromBody] object purchaseData)
        {
            if (Account.isLoggedIn(User))
            {
                Dictionary<string, string> dictPurchaseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(purchaseData.ToString());

                string reference = "Purchase of " + dictPurchaseData["amount"] + " parts of jewelry " + dictPurchaseData["jewelryName"] + ". Total: " + dictPurchaseData["price"] + "$";

                Purchase purchase = new Purchase { Amount = double.Parse(dictPurchaseData["amount"]), Date = DateTime.Now, JewelryId = int.Parse(dictPurchaseData["jewelryId"]), UserId = Account.GetCurrAccountId(User), Reference = reference };

                PurchasesController newPurchase = new PurchasesController(new ColmanInternetiotContext());

                try
                {
                    string country = await countrycode();
                    purchase.Country = country;
                    newPurchase.Create(purchase);
                }
                catch
                {
                    return NotFound();
                }

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<string> countrycode()
        {
            try
            {
                WebResponse res = await HttpWebRequest.Create("https://extreme-ip-lookup.com/json/79.183.11.105").GetResponseAsync();
                string resContent = new StreamReader(res.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(resContent)["countryCode"];
            }
            catch
            {
                return "IL";
            }
        }

        [HttpGet("{user}")]
        public List<Jewelry> GetPurchaseByUser(string user)
        {
            List<Jewelry> jewelries = new List<Jewelry>();

            ColmanInternetiotContext context = new ColmanInternetiotContext();

            List<Purchase> purchases = context.Purchase.Where(x => x.UserId == Account.GetCurrAccountId(User)).ToList();
            
            foreach(Purchase purchase in purchases)
            {
                for(int i = 1; i <= purchase.Amount; i++)
                {
                    Jewelry jewelry = context.Jewelry.First(x => x.Id == purchase.JewelryId);

                    jewelry.Purchase = null;

                    jewelries.Add(jewelry);
                }   
            }

            return jewelries;
        }

        [HttpGet("{month}/{year}/{numOfMonthes}")]
        public Dictionary<string, double> GetProfit(int month, int year, int numOfMonthes)
        {
            string[] lstMonthes = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            Dictionary<string, double> profitPerMonth = new Dictionary<string, double>();
            
            DateTime dt = new DateTime(year, month, 1);

            PurchasesController purchases = new PurchasesController(new ColmanInternetiotContext());

            for (int i = (numOfMonthes - 1); i >= 0; i--)
            {
                DateTime currDate = dt.AddMonths(-i);

                double monthlyProfit = purchases.GetMonthlyProfit(currDate.Month, currDate.Year);

                string strMonthYear = lstMonthes[currDate.Month - 1] + " " + currDate.Year;

                profitPerMonth[strMonthYear] = monthlyProfit;
            }

            return profitPerMonth;
        }
    }
}