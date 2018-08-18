using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            { 
                Models.Account currUser = new Account(HttpContext.User.Claims);

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

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Account()
        {
            ViewData["Message"] = "Your user details.";

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
