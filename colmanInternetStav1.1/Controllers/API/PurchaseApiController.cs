using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Purchase")]
    public class PurchaseApiController : Controller
    {
        [HttpPut]
        public string MakePurchase([FromBody] object purchaseData)
        {
            Dictionary<string, string> dictPurchaseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(purchaseData.ToString());

            string reference = "Purchase of " + dictPurchaseData["amount"] + " parts of jewelry " + dictPurchaseData["jewelryName"] + ". Total: " + dictPurchaseData["price"] + "$";

            Purchase purchase = new Purchase { Amount = double.Parse(dictPurchaseData["amount"]), Date = DateTime.Now, JewelryId = int.Parse(dictPurchaseData["jewelryId"]), UserId = Account.GetCurrAccountId(User), Reference = reference };

            PurchasesController newPurchase = new PurchasesController(new ColmanInternetiotContext());

            string strMessage = "Purchase made successfully !!";

            try
            {
                newPurchase.Create(purchase);
            }
            catch
            {
                strMessage = "Error on making purchase...";
            }

            return strMessage;
        }
    }
}