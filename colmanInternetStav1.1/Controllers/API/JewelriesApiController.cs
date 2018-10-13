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
        public List<Jewelry> GetJewelries()
        {
            return (new ColmanInternetiotContext().Jewelry).ToList();
        }

        [HttpGet("{id}")]
        public Dictionary<string, string> GetJewelries(int id)
        {
            ColmanInternetiotContext _context = new ColmanInternetiotContext();

            Jewelry currJewelry = _context.Jewelry.FirstOrDefault(m => m.Id == id);

            Dictionary<string, string> dictJewelry = new Dictionary<string, string>();
            currJewelry.Category = _context.Category.FirstOrDefault(m => m.Id == currJewelry.CategoryId);
            currJewelry.Set = _context.JewelrySet.FirstOrDefault(m => m.Id == currJewelry.SetId);

            dictJewelry.Add("id", currJewelry.Id.ToString());
            dictJewelry.Add("amount", currJewelry.Amount.ToString());
            dictJewelry.Add("price", currJewelry.Price.ToString());
            dictJewelry.Add("cart", currJewelry.Cart.ToString());
            dictJewelry.Add("name", currJewelry.Name);
            dictJewelry.Add("description", currJewelry.Description);
            dictJewelry.Add("imagePath", currJewelry.ImagePath);
            dictJewelry.Add("size", currJewelry.Size.ToString());
            dictJewelry.Add("weight", currJewelry.Weight.ToString());
            dictJewelry.Add("categoryId", currJewelry.CategoryId.ToString());
            dictJewelry.Add("category", currJewelry.Category.Name);
            dictJewelry.Add("setId", currJewelry.SetId.ToString());
            dictJewelry.Add("set", currJewelry.Set.Name);

            return dictJewelry;
        }

        [HttpGet("{setOrCatagory}/{id}")]
        public List<Jewelry> GetJewelries(string setOrCatagory, int id)
        {
            ColmanInternetiotContext context = new ColmanInternetiotContext();

            List<Jewelry> listJewelriesOnSetOrCatagory = new List<Jewelry>();

            if (setOrCatagory.ToLower() == "set")
            {
                listJewelriesOnSetOrCatagory = context.Jewelry.Where(x => x.SetId == id).ToList();
            }
            else if (setOrCatagory.ToLower() == "category")
            {
                listJewelriesOnSetOrCatagory = context.Jewelry.Where(x => x.CategoryId == id).ToList();
            }

            return listJewelriesOnSetOrCatagory;
        }

        [HttpGet("{catagory}/{price}/{cart}/{diamonds}")]
        public List<Jewelry> GetJewelries(string catagory, double price, int cart, bool diamonds)
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