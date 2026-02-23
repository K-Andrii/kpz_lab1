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
    public class SalesController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public SalesController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var videoGamesCatalogContext = _context.GameSales.Include(g => g.Key).Include(g => g.Order);
            return View(await videoGamesCatalogContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameSale = await _context.GameSales
                .Include(g => g.Key)
                .Include(g => g.Order)
                .FirstOrDefaultAsync(m => m.KeyId == id);
            if (gameSale == null)
            {
                return NotFound();
            }

            return View(gameSale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["KeyId"] = new SelectList(_context.GameKeys, "KeyId", "KeyId");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KeyId,OrderId,PricePaid,DiscountApplied,FinalPrice")] GameSale gameSale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameSale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KeyId"] = new SelectList(_context.GameKeys, "KeyId", "KeyId", gameSale.KeyId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameSale.OrderId);
            return View(gameSale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameSale = await _context.GameSales.FindAsync(id);
            if (gameSale == null)
            {
                return NotFound();
            }
            ViewData["KeyId"] = new SelectList(_context.GameKeys, "KeyId", "KeyId", gameSale.KeyId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameSale.OrderId);
            return View(gameSale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KeyId,OrderId,PricePaid,DiscountApplied,FinalPrice")] GameSale gameSale)
        {
            if (id != gameSale.KeyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameSaleExists(gameSale.KeyId))
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
            ViewData["KeyId"] = new SelectList(_context.GameKeys, "KeyId", "KeyId", gameSale.KeyId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameSale.OrderId);
            return View(gameSale);
        }

        // GET: Sales/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameSale = await _context.GameSales
                .Include(g => g.Key)
                .Include(g => g.Order)
                .FirstOrDefaultAsync(m => m.KeyId == id);
            if (gameSale == null)
            {
                return NotFound();
            }

            return View(gameSale);
        }

        // POST: Sales/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameSale = await _context.GameSales.FindAsync(id);
            if (gameSale != null)
            {
                _context.GameSales.Remove(gameSale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameSaleExists(int id)
        {
            return _context.GameSales.Any(e => e.KeyId == id);
        }
    }
}
