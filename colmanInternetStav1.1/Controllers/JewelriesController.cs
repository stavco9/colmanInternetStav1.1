using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using colmanInternetStav1._1.Models;
using System.Net;

namespace colmanInternetStav1._1.Controllers
{
    public class JewelriesController : Controller
    {
        private readonly ColmanInternetiotContext _context;
        
        public static List<Jewelry> getAllJewelries()
        {
            ColmanInternetiotContext context = new ColmanInternetiotContext();

            return (context.Jewelry.ToList());
        }

        public double getMaxPrice()
        {
            List<double> prices = _context.Jewelry.Select(c => c.Price).ToList();

            return prices.Max<double>();
        }

        public double getMinPrice()
        {
            List<double> prices = _context.Jewelry.Select(c => c.Price).ToList();

            return prices.Min<double>();
        }

        public JewelriesController(ColmanInternetiotContext context)
        {
            _context = context;    
        }

        // GET: Jewelries
        public async Task<IActionResult> Index()
        {
            var colmanInternetiotContext = _context.Jewelry.Include(j => j.Category).Include(j => j.Set);
            return View(await colmanInternetiotContext.ToListAsync());
        }

        // GET: Jewelries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelry = await _context.Jewelry
                .Include(j => j.Category)
                .Include(j => j.Set)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (jewelry == null)
            {
                return NotFound();
            }

            return View(jewelry);
        }

        // GET: Jewelries/Create
        public IActionResult Create()
        {
            if (Account.isAdmin(User))
            {
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
                ViewData["SetId"] = new SelectList(_context.JewelrySet, "Id", "Name");
                return View();
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Jewelries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Weight,Price,Cart,Size,Description,Amount,Discount,Id,Diamonds,ImagePath,Name,CategoryId,SetId")] Jewelry jewelry)
        {
            if (Account.isAdmin(User))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(jewelry);
                    await _context.SaveChangesAsync();

                    string postToFacebook = ("התכשיט " + jewelry.Name + " נוסף לחנות. יש " + jewelry.Amount + " פריטים. היכנסו לאתר בשביל לקנות");

                    FacebookPost post = new FacebookPost();

                    post.PublishToFacebookNoPhoto(postToFacebook);

                    return RedirectToAction("Index");
                }
           
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", jewelry.CategoryId);
                ViewData["SetId"] = new SelectList(_context.JewelrySet, "Id", "Name", jewelry.SetId);
                return View(jewelry);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // GET: Jewelries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Account.isAdmin(User))
            {
                var jewelry = await _context.Jewelry.SingleOrDefaultAsync(m => m.Id == id);
                if (jewelry == null)
                {
                    return NotFound();
                }
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", jewelry.CategoryId);
                ViewData["SetId"] = new SelectList(_context.JewelrySet, "Id", "Name", jewelry.SetId);
                return View(jewelry);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Jewelries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Weight,Price,Cart,Size,Description,Amount,Discount,Id,Diamonds,ImagePath,Name,CategoryId,SetId")] Jewelry jewelry)
        {
            if (id != jewelry.Id)
            {
                return NotFound();
            }

            if (Account.isAdmin(User))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ColmanInternetiotContext newContext = new ColmanInternetiotContext();

                        int? currAmount = newContext.Jewelry.First(x => x.Id == jewelry.Id).Amount;

                        _context.Update(jewelry);
                        await _context.SaveChangesAsync();

                        if (currAmount != null)
                        {
                            string postToFacebook = ("לתכשיט " + jewelry.Name + " נוספו " + (jewelry.Amount - int.Parse(currAmount.ToString())) + " פריטים חדשים. היכנסו לאתר בשביל לקנות");

                            FacebookPost post = new FacebookPost();

                            post.PublishToFacebookNoPhoto(postToFacebook);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JewelryExists(jewelry.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index");
                }
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", jewelry.CategoryId);
                ViewData["SetId"] = new SelectList(_context.JewelrySet, "Id", "Name", jewelry.SetId);
                return View(jewelry);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // GET: Jewelries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Account.isAdmin(User))
            {
                var jewelry = await _context.Jewelry
                .Include(j => j.Category)
                .Include(j => j.Set)
                .SingleOrDefaultAsync(m => m.Id == id);
                if (jewelry == null)
                {
                    return NotFound();
                }

                return View(jewelry);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Jewelries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Account.isAdmin(User))
            {
                var jewelry = await _context.Jewelry.SingleOrDefaultAsync(m => m.Id == id);
                _context.Jewelry.Remove(jewelry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        private bool JewelryExists(int id)
        {
            return _context.Jewelry.Any(e => e.Id == id);
        }
    }
}
