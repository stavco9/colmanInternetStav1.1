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
            return (new ColmanInternetiotContext().Jewelry.Where(m => m.Amount > 0)).ToList();
        }

        [HttpGet("{id}")]
        public object GetJewelries(int id)
        {
            ColmanInternetiotContext _context = new ColmanInternetiotContext();

            var result = from jw in _context.Jewelry
                join ctgr in _context.Category on jw.CategoryId equals ctgr.Id
                join jwset in _context.JewelrySet on jw.SetId equals jwset.Id
                where jw.Id == id
                select new
                {
                    jw.Id,
                    jw.Amount,
                    jw.Price,
                    jw.Cart,
                    jw.Name,
                    jw.Description,
                    jw.ImagePath,
                    jw.Size,
                    jw.Weight,
                    jw.CategoryId,
                    category = ctgr.Name,
                    jw.SetId,
                    set = jwset.Name
                };

            return result.FirstOrDefault();
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
                listOthers = context.Jewelry.Where(x => x.Price >= price && x.Diamonds == diamonds && x.Amount > 0).ToList();
            }
            else
            {
                listOthers = context.Jewelry.Where(x => x.Price >= price && x.Amount > 0).ToList();
            }

            return listCategories.Intersect(listCart).Intersect(listOthers).ToList();
        }
    }
}