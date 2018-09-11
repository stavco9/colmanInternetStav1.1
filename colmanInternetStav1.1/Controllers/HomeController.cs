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
            if (User.Identity.IsAuthenticated)
            {
                Models.Account account = Models.Account.GetCurrAccount(HttpContext.User.Claims);

                ViewData["UserInfo"] = account;

                UsersController addToDb = new UsersController(new ColmanInternetiotContext());

                var userToDb = new Users { NameId = account.NameID, Email = account.EmailAddress, FName = account.FirstName, LName = account.LastName, Name = account.FullName, Gender = account.Gender, IsAdmin = false };

                addToDb.Create(userToDb);

                return View();
            }
            else
            {
                return FacebookLogin();
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            return View();
        }

        public IActionResult Location()
        {
            ViewData["Message"] = "Store Locations";
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            return View();
        }

        public IActionResult Account()
        {
            ViewData["Message"] = "Your user details.";
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            return View();
        }

        public IActionResult NotAuthorized()
        {
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            return View();
        }

        public IActionResult FacebookLogin()
        {
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            };

            return Challenge(authProperties, "Facebook");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
