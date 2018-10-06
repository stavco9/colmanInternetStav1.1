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

        public static int CreatedTodayCount()
        {
            ColmanInternetiotContext _context = new ColmanInternetiotContext();

            int countOfCraetedToday = _context.Users.Where(c => c.CreationDate.Value.Date == DateTime.Now.Date).Count();

            return countOfCraetedToday;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (!Account.isLoggedIn(User))
            {
                return (new RedirectToActionResult("Login", "Profile", null));
            }

            if (!Account.isAdmin(User))
            {
                return (new RedirectToActionResult("Index", "Home", null));
            }

            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async void Create([Bind("Id,Email,FName,LName,Name,Gender,IsAdmin,CreationDate")] Users users)
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
            if (Account.isAdmin(HttpContext.User))
            {
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

            return (new RedirectToActionResult("NotAuthorized", "Home", null));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameId,Email,FName,LName,Name,Gender,IsAdmin,CreationDate")] Users users)
        {
            if (Account.isLoggedIn(HttpContext.User))
            {
                if (Account.isAdmin(HttpContext.User))
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
            }
           
            return (new RedirectToActionResult("NotAuthorized", "Home", null));
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
