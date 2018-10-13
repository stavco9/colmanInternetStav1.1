using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using colmanInternetStav1._1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/AdminApi")]
    public class AdminApiController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public async Task<string> CountryPurchases()
        {
            Dictionary<string, double?> countriesByPurchases = new Dictionary<string, double?>();

            foreach (var currPurchase in _context.Purchase)
            {
                if (countriesByPurchases.ContainsKey(currPurchase.Country))                     
                {
                    if ((currPurchase.Date.Value < DateTime.Now.AddYears(-1)))
                    {
                        countriesByPurchases[currPurchase.Country] += currPurchase.Amount;
                    }                    
                }
                else
                {
                    countriesByPurchases.Add(currPurchase.Country, currPurchase.Amount);
                }
            }

            countriesByPurchases.OrderByDescending(key => key.Value);

            return JsonConvert.SerializeObject(countriesByPurchases);
        }

        public async Task<string> ProfitByMonth()
        {
            Dictionary<string, double?> profitByMonthDictionary = new Dictionary<string, double?>();
            double priceForAbaPich;

            foreach (var currPurchase in _context.Purchase)
            {
                string currNormalizedDate = currPurchase.Date.Value.ToString("yyyy-M-d dddd");

                currNormalizedDate = currNormalizedDate.Remove(currNormalizedDate.IndexOf('-', 5));

                if (profitByMonthDictionary.ContainsKey(currNormalizedDate))
                {
                    if ((currPurchase.Date.Value.AddDays(-currPurchase.Date.Value.Day) <
                         DateTime.Now.AddYears(-1).AddDays(-DateTime.Now.Day)))
                    {
                        priceForAbaPich = 0;
                        profitByMonthDictionary[currNormalizedDate] +=
                            currPurchase.Jewelry.Price * currPurchase.Jewelry.Discount - priceForAbaPich;
                    }
                }
                else
                {
                    priceForAbaPich = 0;
                    profitByMonthDictionary.Add(currNormalizedDate,
                        currPurchase.Jewelry.Price * currPurchase.Jewelry.Discount - priceForAbaPich);
                }
            }

            profitByMonthDictionary.OrderByDescending(key => key.Key);

            return JsonConvert.SerializeObject(profitByMonthDictionary);
        }

        public async Task<string> NewMonthyUsers()
        {
            int newUsersCount = 0;

            foreach (var currUser in _context.Users)
            {
                string currNormalizedDate = currUser.CreationDate.Value.ToString("yyyy-M-d dddd");
                string todayNormalizedDate = DateTime.Now.ToString("yyyy-M-d dddd");

                currNormalizedDate = currNormalizedDate.Remove(currNormalizedDate.IndexOf('-', 5));
                todayNormalizedDate = todayNormalizedDate.Remove(currNormalizedDate.IndexOf('-', 5));

                if (currNormalizedDate == todayNormalizedDate)
                {
                    newUsersCount++;
                }
            }

            return 
        }

        public async Task<string> ProfitToday()
        {            
            double? priceForAbaPich;
            double? todayProfit = 0;

            foreach (var currPurchase in _context.Purchase)
            {                                
                if (currPurchase.Date.Value.ToString("d").Equals(DateTime.Now.ToString("d")))                         
                {
                    priceForAbaPich = 0;
                    todayProfit +=
                        currPurchase.Jewelry.Price * currPurchase.Jewelry.Discount - priceForAbaPich;
                }                               
            }

            return JsonConvert.SerializeObject(todayProfit);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {            
            Dictionary<string, string> statisticsDictionary = new Dictionary<string, string>();

            string profByMonth   = await ProfitByMonth();
            string profToday     = await ProfitToday();
            string purcByCountry = await CountryPurchases();
            string newMontUsers  = await NewMonthyUsers();

            statisticsDictionary.Add("ProfitToday", profToday);
            statisticsDictionary.Add("ProfitTByMonth", profByMonth);
            statisticsDictionary.Add("PurchasesOfCountries", purcByCountry);
            statisticsDictionary.Add("NewMonthyUsers", newMontUsers);

            return Json(statisticsDictionary);
        }
    }
}