using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VideoGamesCatalogApp.Models;

namespace VideoGamesCatalogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public UsersController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "balance_desc" : "Balance";

            ViewData["LastLoginSortParm"] = sortOrder == "LoginDate" ? "login_desc" : "LoginDate";

            ViewData["CurrentFilter"] = searchString;

            var users = _context.Users.Include(u => u.Role).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Username.Contains(searchString) || u.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc": users = users.OrderByDescending(u => u.Username); break;
                case "Date": users = users.OrderBy(u => u.RegistrationDate); break;
                case "date_desc": users = users.OrderByDescending(u => u.RegistrationDate); break;

                case "Balance": users = users.OrderBy(u => u.WalletBalance); break;
                case "balance_desc": users = users.OrderByDescending(u => u.WalletBalance); break;

                case "LoginDate": users = users.OrderBy(u => u.LastLoginDate); break;
                case "login_desc": users = users.OrderByDescending(u => u.LastLoginDate); break;

                default: users = users.OrderBy(u => u.Username); break;
            }

            return View(await users.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,PasswordHash,FirstName,LastName,Phone,WalletBalance,RegistrationDate,LastLoginDate,IsBlocked,BanReason,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,PasswordHash,FirstName,LastName,Phone,WalletBalance,RegistrationDate,LastLoginDate,IsBlocked,BanReason,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        public async Task<IActionResult> Statistics()
        {
            var stats = await _context.Users
                .GroupBy(u => u.Role.RoleName)
                .Select(g => new {
                    Role = g.Key,
                    Count = g.Count(),
                    AvgBalance = g.Average(u => u.WalletBalance)
                }).ToListAsync();

            return View(stats);
        }
    }
}
