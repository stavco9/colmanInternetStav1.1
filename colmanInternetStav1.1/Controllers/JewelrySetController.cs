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
    public class JewelrySetController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public JewelrySetController(ColmanInternetiotContext context)
        {
            _context = context;    
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (Account.isAdmin(User))
            {
                return View(await _context.JewelrySet.ToListAsync());
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
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
        public async Task<IActionResult> Create([Bind("Id,Name")] JewelrySet jewelrySet)
        {
            if (Account.isAdmin(User))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(jewelrySet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(jewelrySet);
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

                var jewelryset = await _context.JewelrySet
                        .SingleOrDefaultAsync(m => m.Id == id);
                if (jewelryset == null)
                {
                    return NotFound();
                }

                return View(jewelryset);
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
                var jewelrySet = await _context.JewelrySet.SingleOrDefaultAsync(m => m.Id == id);
                _context.JewelrySet.Remove(jewelrySet);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        private bool JewelrtSetExists(int id)
        {
            return _context.JewelrySet.Any(e => e.Id == id);
        }
    }
}
