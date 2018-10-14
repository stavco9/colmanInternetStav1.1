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

                var result = from crrUsrPrcs in currUser.Purchase
                             let newObj = new
                             {
                                 crrUsrPrcs.Jewelry.Name,
                                 crrUsrPrcs.Jewelry.Description,
                                 crrUsrPrcs.Jewelry.Id,
                                 ActualPrice = crrUsrPrcs.Jewelry.Price * crrUsrPrcs.Jewelry.Discount,
                                 PurchaseDate = crrUsrPrcs.Date
                             }
                             select newObj;                               

                return Json(result);
            }

            return (new RedirectToActionResult("Login", "Profile", null));
        }
    }
}