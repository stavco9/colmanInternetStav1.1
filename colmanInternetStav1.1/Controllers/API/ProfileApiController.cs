using System;
using System.Collections;
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
    [Route("api/ProfileApi")]
    public class ProfileApiController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            if (Account.isLoggedIn(User))
            {
                ColmanInternetiotContext context = new ColmanInternetiotContext();

                Users currUser = await context.Users.FindAsync(Account.getDetails(User)["nameid"]);

                ArrayList allPurchases = new ArrayList();

                foreach (var currPurchase in currUser.Purchase)
                {
                    allPurchases.Add(new
                    {
                        Name = currPurchase.Jewelry.Name,
                        Description = currPurchase.Jewelry.Description,
                        Id = currPurchase.Jewelry.Id,
                        ActualPrice = currPurchase.Jewelry.Price * currPurchase.Jewelry.Discount,
                        PurchaseDate = currPurchase.Date
                    });                  
                }

                return Json(allPurchases);
            }

            return (new RedirectToActionResult("Login", "Profile", null));
        }

    }
}