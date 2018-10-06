using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using colmanInternetStav1._1.Models;

namespace colmanInternetStav1._1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public CategoriesController(ColmanInternetiotContext context)
        {
            _context = context;    
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (Account.isAdmin(User))
            {
                return View(await _context.Category.ToListAsync());
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        public static Dictionary<int, string> GetCategories()
        {
            Dictionary<int, string> categoriesDict = new Dictionary<int, string>();

            List<Category> categoriesFromDB = new ColmanInternetiotContext().Category.ToList();

            foreach(Category category in categoriesFromDB)
            {
                categoriesDict[category.Id] = category.Name;
            }

            return categoriesDict;
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            if (Account.isAdmin(User))
            {
                return View();
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (Account.isAdmin(User))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }
     
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Account.isAdmin(User))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Category
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Account.isAdmin(User))
            {
                var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
