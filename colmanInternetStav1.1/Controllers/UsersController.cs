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
    public class UsersController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public UsersController(ColmanInternetiotContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            if (Account.IsCurrUserAdmin(HttpContext.User.Claims, _context))
            {
                return View(await _context.Users.ToListAsync());
            }

            return (new RedirectToActionResult("NotAuthorized", "Home", null));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        public IEnumerable<Users> GetUsers()
        {
            return _context.Users.ToList();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void Create([Bind("Id,Email,FName,LName,Name,Gender,IsAdmin")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (!(UsersExists(users.NameId)))
                {
                    _context.Add(users);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["UserInfo"] = Models.Account.GetCurrAccount(HttpContext.User.Claims);

            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FName,LName,Name,Gender,IsAdmin")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.NameId))
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
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UsersExists(string nameID)
        {
            return _context.Users.Any(e => e.NameId == nameID);
        }
    }
}
