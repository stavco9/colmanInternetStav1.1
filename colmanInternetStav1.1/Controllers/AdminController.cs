using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (!Account.isLoggedIn(User))
            {
                return (new RedirectToActionResult("Login", "Profile", null));
            }

            if (!Account.isAdmin(User))
            {
                return (new RedirectToActionResult("Index", "Home", null));
            }

            return View();
        }
    }
}