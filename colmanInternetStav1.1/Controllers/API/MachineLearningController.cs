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
public class MachineLearningController : Controller
{
        // GET: api/<controller>
        [HttpGet]
        public async Task<Jewelry> Get()
        {
            ColmanInternetiotContext dbcontext = new ColmanInternetiotContext();
            string currentUserCountry = await new PurchaseApiController().countrycode();
            string currentUserGender = (Account.isLoggedIn(User) ? Account.getDetails(User)["gender"] : "female").ToLower();
            Random rnd = new Random();

            var dataset = dbcontext.Purchase.OrderByDescending(p => p.Id).Take(100);
            var lables = dbcontext.JewelrySet.Select(set => set.Id).ToArray();
            Dictionary<int, double> probabilities = new Dictionary<int, double>();
            int datasetSize = dataset.Count();
            if (datasetSize > 0 && lables.Length > 0)
            {
                foreach (int label in lables)
                {
                    var subset = dataset.Where(p => p.Jewelry.SetId == label);
                    int subsetSize = subset.Count();
                    if (subsetSize > 0)
                    {
                        double prior = (double)subsetSize / (double)datasetSize;
                        double likelihood = 1;
                        likelihood *= (double)(subset.Where(p => p.Country == currentUserCountry).Count()) / (double)subsetSize;
                        likelihood *= (double)(subset.Where(p => p.User.Gender.ToLower() == currentUserGender).Count()) / (double)subsetSize;

                        probabilities[label] = prior * likelihood;
                    }
                    else
                    {
                        probabilities[label] = 0;
                    }
                }

                int selectedSet = probabilities.OrderByDescending(p => p.Value).First().Key;

                // select a random jewrley within the set
                var setJewrleys = dbcontext.Jewelry.Where(j => j.SetId == selectedSet).ToArray();
                return (setJewrleys[rnd.Next(setJewrleys.Length)]);
            }

            // if the dataset not contains data, return a random jewrley
            var allJewrleys = dbcontext.Jewelry.ToArray();
            return (allJewrleys[rnd.Next(allJewrleys.Length)]);
        }

        
}
}
