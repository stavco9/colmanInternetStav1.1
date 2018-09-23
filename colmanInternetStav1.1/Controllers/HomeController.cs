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
            if (Account.isLoggedIn(User))
            {
                UsersController addNewUser = new UsersController(new ColmanInternetiotContext());

                Dictionary<string, string> userDetails = Account.getDetails(User);

                var newUserToDB = new Users { NameId = userDetails["nameid"], Email = userDetails["emailaddress"], FName = userDetails["firstname"], LName = userDetails["lastname"], Name = userDetails["fullname"], Gender = userDetails["gender"], IsAdmin = false };

                addNewUser.Create(newUserToDB);
            }
    
            return View();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Location()
        {
            return View();
        }
    }
}
