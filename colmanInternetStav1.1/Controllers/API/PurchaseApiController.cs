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
using System.Reflection;

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

        public async Task<string> countrycode()
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

        //This method returns a list of purchases made by the current account
        [HttpGet("{user}")]
        public List<Dictionary<string, object>> GetPurchaseByUser(string user)
        {
            List<Dictionary<string, object>> jewelries = new List<Dictionary<string, object>>();

            ColmanInternetiotContext context = new ColmanInternetiotContext();

            // Get list of current account purchases
            List<Purchase> purchases = context.Purchase.Where(x => x.UserId == Account.GetCurrAccountId(User)).ToList();
            
            foreach(Purchase purchase in purchases)
            {
                Dictionary<string, object> dictJewelry = new Dictionary<string, object>();

                //Get the jewelry object of the jewelry that bought in this purchase
                Jewelry jewelry = context.Jewelry.First(x => x.Id == purchase.JewelryId);

                jewelry.Purchase = null;

                // Convert from Jewelry object to Dictionary
                dictJewelry = jewelry.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name.ToLower(), prop => prop.GetValue(jewelry, null));

                // Add a summary attribute
                dictJewelry.Add("summary", purchase.Amount + " X " + jewelry.Price);

                jewelries.Add(dictJewelry);
            }

            return jewelries;
        }

        // This method gets a list of monthly profits from the given month
        // The method starts with the month and year given as input, and 
        // measures the profit of each month in the last "numOfMonthes" from the given month
        [HttpGet("{month}/{year}/{numOfMonthes}")]
        public Dictionary<string, double> GetProfit(int month, int year, int numOfMonthes)
        {
            string[] lstMonthes = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            Dictionary<string, double> profitPerMonth = new Dictionary<string, double>();
            
            // Start with the month and year given as input
            DateTime dt = new DateTime(year, month, 1);

            PurchasesController purchases = new PurchasesController(new ColmanInternetiotContext());

            // For the last "numOfMonthes" before the month given as input
            for (int i = (numOfMonthes - 1); i >= 0; i--)
            {
                // Get the current month in the loop
                DateTime currDate = dt.AddMonths(-i);

                // Get the monthly profit of the current month
                double monthlyProfit = purchases.GetMonthlyProfit(currDate.Month, currDate.Year);

                // Build the display name of current month and year in the loop
                string strMonthYear = lstMonthes[currDate.Month - 1] + " " + currDate.Year;
                
                // Set the value of the profit of the current month
                profitPerMonth[strMonthYear] = monthlyProfit;
            }

            // Return the dictionary
            return profitPerMonth;
        }
    }
}