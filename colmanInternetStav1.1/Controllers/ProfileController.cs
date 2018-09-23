using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if (!Account.isLoggedIn(User))
            {
                return (new RedirectToActionResult("Login", "Profile", null));
            }
            return View();
        }

        public IActionResult ResyncFromFacebook()
        {
            // resync from facebook, somehow,
            // the redirect to profile page

            return (new RedirectToActionResult("Index", "Profile", null));
        }

        public IActionResult Login()
        {
            if (Account.isLoggedIn(User))
            {
                return (new RedirectToActionResult("Index", "Profile", null));
            }
            else
            {
                // facebook login
                var authProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                };

                return Challenge(authProperties, "Facebook");
            }
        }

        public IActionResult Logout()
        {
            if (Account.isLoggedIn(User))
            {
                Response.Cookies.Delete(".AspNetCore.ApplicationCookie");
            }

            return (new RedirectToActionResult("Index", "Home", null));
        }
    }
}