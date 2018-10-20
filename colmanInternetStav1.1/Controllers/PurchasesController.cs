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
    public class PurchasesController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public PurchasesController(ColmanInternetiotContext context)
        {
            _context = context;    
        }

        public double GetDailyProfit()
        {
            double? numOfPurchases = _context.Purchase.Where(x => x.Date == DateTime.Today.Date).Sum(m => getProfitFromPurchase(m.JewelryId, double.Parse(m.Amount.ToString())));

            if (numOfPurchases == null)
            {
                return 0;
            }

            return double.Parse(numOfPurchases.ToString());
        }

        public double GetMonthlyProfit(int month, int year)
        {
            double? numOfPurchases = _context.Purchase.Where(x => x.Date.Value.Month == month && x.Date.Value.Year == year).Sum(m => getProfitFromPurchase(m.JewelryId, double.Parse(m.Amount.ToString())));

            if (numOfPurchases == null)
            {
                return 0;
            }

            return double.Parse(numOfPurchases.ToString());
        }

        private double getProfitFromPurchase(int jewelryId, double amount)
        {
            return (amount * new ColmanInternetiotContext().Jewelry.First(x => x.Id == jewelryId).Price);
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var colmanInternetiotContext = _context.Purchase.Include(p => p.Jewelry).Include(p => p.User);
            return View(await colmanInternetiotContext.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Include(p => p.Jewelry)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["JewelryId"] = new SelectList(_context.Jewelry, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void Create([Bind("JewelryId,UserId,Date,Amount,Reference,Id,Country")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();

                Jewelry jewelry = _context.Jewelry.First(x => x.Id == purchase.JewelryId);

                jewelry.Amount -= (int)purchase.Amount;

                _context.Jewelry.Update(jewelry);
                await _context.SaveChangesAsync();

            }
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["JewelryId"] = new SelectList(_context.Jewelry, "Id", "Name", purchase.JewelryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", purchase.UserId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JewelryId,UserId,Date,Amount,Reference,Id,Country")] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Id))
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
            ViewData["JewelryId"] = new SelectList(_context.Jewelry, "Id", "Name", purchase.JewelryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", purchase.UserId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Include(p => p.Jewelry)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchase.SingleOrDefaultAsync(m => m.Id == id);
            _context.Purchase.Remove(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.Id == id);
        }
    }
}
