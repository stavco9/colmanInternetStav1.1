using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using colmanInternetStav1._1.Models;
using Microsoft.AspNetCore.Mvc;

namespace colmanInternetStav1._1.Controllers
{
    public class JewelryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}