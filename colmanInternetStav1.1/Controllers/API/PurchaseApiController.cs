using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using colmanInternetStav1._1.Models;
using System.Reflection;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Purchase")]
    public class PurchaseApiController : Controller
    {
        [HttpPut]
        public IActionResult MakePurchase([FromBody] object purchaseData)
        {
            if (Account.isLoggedIn(User))
            {
                Dictionary<string, string> dictPurchaseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(purchaseData.ToString());

                string reference = "Purchase of " + dictPurchaseData["amount"] + " parts of jewelry " + dictPurchaseData["jewelryName"] + ". Total: " + dictPurchaseData["price"] + "$";

                Purchase purchase = new Purchase { Amount = double.Parse(dictPurchaseData["amount"]), Date = DateTime.Now, JewelryId = int.Parse(dictPurchaseData["jewelryId"]), UserId = Account.GetCurrAccountId(User), Reference = reference };

                PurchasesController newPurchase = new PurchasesController(new ColmanInternetiotContext());

                try
                {
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

        [HttpGet("{user}")]
        public List<Dictionary<string, object>> GetPurchaseByUser(string user)
        {
            List<Dictionary<string, object>> jewelries = new List<Dictionary<string, object>>();

            ColmanInternetiotContext context = new ColmanInternetiotContext();

            List<Purchase> purchases = context.Purchase.Where(x => x.UserId == Account.GetCurrAccountId(User)).ToList();
            
            foreach(Purchase purchase in purchases)
            {
                Dictionary<string, object> dictJewelry = new Dictionary<string, object>();

                Jewelry jewelry = context.Jewelry.First(x => x.Id == purchase.JewelryId);

                jewelry.Purchase = null;

                dictJewelry = jewelry.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name.ToLower(), prop => prop.GetValue(jewelry, null));

                dictJewelry.Add("summary", purchase.Amount + " X " + jewelry.Price);

                jewelries.Add(dictJewelry);
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