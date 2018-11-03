using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using colmanInternetStav1._1.Controllers;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Sets")]
    public class SetsApiController : Controller
    {
        // This function returns a list of the sets with the number of jewelries on each set
        [HttpGet("{count}")]
        public Dictionary<string, int> getNumOfJewelriesInSet(string count)
        {
            ColmanInternetiotContext context = new ColmanInternetiotContext();

            Dictionary<string, int> dictJewelriesInSet = new Dictionary<string, int>();

            // Get a list of number of jewelries per set by group by query from DB
            var result = (from je in context.Jewelry
                          join jewSet in context.JewelrySet on je.SetId equals jewSet.Id
                          group je by jewSet.Name into cntJewls
                          select new
                          {
                              setName = cntJewls.Key,
                              cntJewelries = cntJewls.ToList() 
                          });

            // Convery the result to dictionary
            foreach(var line in result)
            {
                dictJewelriesInSet[line.setName] = line.cntJewelries.Count;
            }

            // Return the dictionary
            return dictJewelriesInSet;
        }
    }
}