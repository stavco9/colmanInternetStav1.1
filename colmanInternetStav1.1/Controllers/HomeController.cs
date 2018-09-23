using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using colmanInternetStav1._1.Models;
using System.Security.Claims;

namespace colmanInternetStav1._1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            /*Models.Account account = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            ViewData["UserInfo"] = account;

            if (User.Identity.IsAuthenticated)
            {
                UsersController addToDb = new UsersController(new ColmanInternetiotContext());

                var userToDb = new Users { NameId = account.NameID, Email = account.EmailAddress, FName = account.FirstName, LName = account.LastName, Name = account.FullName, Gender = account.Gender, IsAdmin = false };

                addToDb.Create(userToDb);

            }*/
    
            return View();
        }
    }
}
