using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using colmanInternetStav1._1.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace colmanInternetStav1._1.Controllers.API
{
    [Route("api/[controller]")]
public class AdminController : Controller
{
        // GET: api/<controller>
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            if (!Account.isAdmin(User))
            {
                return (null);
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            ColmanInternetiotContext dbcontext = new ColmanInternetiotContext();
            DateTime today = DateTime.Today;
            DateTime quarterBeginning = new DateTime(today.Year, (today.Month - 1) / 3 * 3 + 1, 1);

            result.Add("earnings_since_the_beginning_of_the_day",
                dbcontext.Purchase.Where(p => DateTime.Compare(today, p.Date.Value) <= 0)
                .Select(p => p.Amount).Sum().ToString());

            result.Add("earnings_since_the_beginning_of_the_quarter",
                dbcontext.Purchase.Where(p => DateTime.Compare(quarterBeginning, p.Date.Value) <= 0)
                .Select(p => p.Amount).Sum().ToString());

            result.Add("earnings_since_the_beginning_of_the_quarter_by_country",
                JsonConvert.SerializeObject(dbcontext.Purchase.Where(p => p.Country != null && p.Date != null && DateTime.Compare(quarterBeginning, p.Date.Value) <= 0)
                .GroupBy(p => p.Country).Select(g => new
                {
                    Country = g.Key,
                    Earnings = g.Select(p => p.Amount).Sum()
                }).OrderByDescending(g => g.Earnings).ToList()));

            result.Add("earnings_at_last_14_months_by_month",
                JsonConvert.SerializeObject(dbcontext.Purchase.Where(p => DateTime.Compare(today.AddMonths(-14), p.Date.Value) <= 0)
                .GroupBy(p => p.Date.Value.Month+"."+ p.Date.Value.Year).Select(g => new
                {
                    Month = g.Key,
                    Earnings = g.Select(p => p.Amount).Sum()

                }).OrderByDescending(g => g.Earnings).ToList()));

            result.Add("10_last_purchases",
                JsonConvert.SerializeObject(dbcontext.Purchase.OrderByDescending(p => p.Id).Take(10).Select(p => new
                {
                    jewrley = p.Jewelry.Name,
                    client = p.User.FName + " " + p.User.LName,
                    money = p.Jewelry.Price * p.Amount,
                    img = p.Jewelry.ImagePath
                }).ToList()));

            return (result);
        }
}
}
