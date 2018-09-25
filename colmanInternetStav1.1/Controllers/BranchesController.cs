using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using colmanInternetStav1._1.Models;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;

namespace colmanInternetStav1._1.Controllers
{
    public class BranchesController : Controller
    {
        private readonly ColmanInternetiotContext _context;

        public bool isStoreOwner(ClaimsPrincipal principal, int storeId)
        {
            if (Account.isLoggedIn(User))
            {
                ColmanInternetiotContext context = new ColmanInternetiotContext();

                int userId = context.Users.FirstOrDefault(c => c.NameId == Account.getDetails(principal)["nameid"]).Id;

                int? managerId = context.Branch.FirstOrDefault(c => c.Id == storeId).ManagerId;

                if (managerId != null)
                {
                    return (userId == managerId);
                }
            }

            return false;
        }

        public BranchesController(ColmanInternetiotContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            var colmanInternetiotContext = _context.Branch.Include(b => b.Manager);
            return View(await colmanInternetiotContext.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .Include(b => b.Manager)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            if (Account.isAdmin(User))
            {
                ViewData["ManagerId"] = new SelectList(_context.Users, "Id", "Name");
                ViewData["ManagerName"] = new SelectList(_context.Users, "Name", "Name");
                return View();
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,CoordinateLat,CoordinateLng,ManagerId,Id")] Branch branch)
        {
            if (Account.isAdmin(User))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(branch);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewData["ManagerId"] = new SelectList(_context.Users, "Id", "Id", branch.ManagerId);

                return View(branch);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Account.isAdmin(User) || isStoreOwner(User, int.Parse(id.ToString())))
            {
                var branch = await _context.Branch.SingleOrDefaultAsync(m => m.Id == id);
                if (branch == null)
                {
                    return NotFound();
                }

                ViewData["ManagerName"] = _context.Users.FirstOrDefault(c => c.Id == _context.Branch.FirstOrDefault(x => x.Id == int.Parse(id.ToString())).ManagerId).Name;

                ViewData["ManagerId"] = new SelectList(_context.Users, "Id", "Name", branch.ManagerId);
                return View(branch);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Address,CoordinateLat,CoordinateLng,ManagerId,Id")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (Account.isAdmin(User) || isStoreOwner(User, id))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(branch);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BranchExists(branch.Id))
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

                ViewData["ManagerId"] = new SelectList(_context.Users, "Id", "Id", branch.ManagerId);
                return View(branch);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Account.isAdmin(User) || isStoreOwner(User, int.Parse(id.ToString())))
            {
                ViewData["StoreName"] = _context.Branch.FirstOrDefault(c => c.Id == int.Parse(id.ToString())).Name;

                var branch = await _context.Branch
            .Include(b => b.Manager)
            .SingleOrDefaultAsync(m => m.Id == id);
                if (branch == null)
                {
                    return NotFound();
                }

                return View(branch);
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }

        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Account.isAdmin(User) || isStoreOwner(User, id))
            {
                var branch = await _context.Branch.SingleOrDefaultAsync(m => m.Id == id);
                _context.Branch.Remove(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return (new RedirectToActionResult("NotAuthorized", "Home", null));
            }
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
