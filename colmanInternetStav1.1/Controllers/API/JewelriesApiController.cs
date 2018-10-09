using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers
{
    [Produces("application/json")]
    [Route("api/Jewelries")]
    public class JewelriesApiController : Controller
    {
        [HttpGet]
        public List<Jewelry> GetAllJewelries()
        {
            return (new ColmanInternetiotContext().Jewelry).ToList();
        }

        [HttpGet("{catagory}/{price}/{cart}/{diamonds}")]
        public List<Jewelry> GetAllJewelries(string catagory, double price, int cart, bool diamonds)
        {
            ColmanInternetiotContext context = new ColmanInternetiotContext();

            List<Jewelry> listCategories = new List<Jewelry>();

            if (catagory != "all")
            {
                listCategories = context.Jewelry.Where(x => x.CategoryId == (context.Category.FirstOrDefault(c => c.Name == catagory)).Id).ToList();
            }
            else
            {
                listCategories = context.Jewelry.ToList();
            }

            List<Jewelry> listCart = new List<Jewelry>();

            if (cart != 0)
            {
                listCart = context.Jewelry.Where(x => x.Cart == cart).ToList();
            }
            else
            {
                listCart = context.Jewelry.ToList();
            }

            List<Jewelry> listOthers = new List<Jewelry>();

            if (diamonds)
            {
                listOthers = context.Jewelry.Where(x => x.Price >= price && x.Diamonds == diamonds).ToList();
            }
            else
            {
                listOthers = context.Jewelry.Where(x => x.Price >= price).ToList();
            }

            return listCategories.Intersect(listCart).Intersect(listOthers).ToList();
        }
    }
}