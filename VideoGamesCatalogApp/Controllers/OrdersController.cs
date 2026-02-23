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
    public class OrdersController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public OrdersController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["AmountSortParm"] = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewData["UserSortParm"] = sortOrder == "User" ? "user_desc" : "User";

            ViewData["CurrentFilter"] = searchString;

            var orders = _context.Orders.Include(o => o.User).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.OrderStatus.Contains(searchString)
                                        || o.TransactionId.Contains(searchString)
                                        || o.User.Username.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_asc":
                    orders = orders.OrderBy(o => o.OrderDate);
                    break;
                case "Amount":
                    orders = orders.OrderBy(o => o.TotalAmount);
                    break;
                case "amount_desc":
                    orders = orders.OrderByDescending(o => o.TotalAmount);
                    break;
                case "User":
                    orders = orders.OrderBy(o => o.User.Username);
                    break;
                case "user_desc":
                    orders = orders.OrderByDescending(o => o.User.Username);
                    break;
                default:
                    orders = orders.OrderByDescending(o => o.OrderDate);
                    break;
            }

            ViewBag.TotalAmount = await orders.SumAsync(o => o.TotalAmount);
            ViewBag.OrdersCount = await orders.CountAsync();
            ViewBag.AverageCheck = await orders.CountAsync() > 0 ? await orders.AverageAsync(o => o.TotalAmount) : 0;

            return View(await orders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,PaymentMethod,OrderStatus,TotalAmount,TransactionId,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,PaymentMethod,OrderStatus,TotalAmount,TransactionId,UserId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
