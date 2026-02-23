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
    public class KeysController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public KeysController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        // GET: Keys
        public async Task<IActionResult> Index()
        {
            var videoGamesCatalogContext = _context.GameKeys.Include(g => g.CurrentUser).Include(g => g.Game).Include(g => g.Order).Include(g => g.Storefront);
            return View(await videoGamesCatalogContext.ToListAsync());
        }

        // GET: Keys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameKey = await _context.GameKeys
                .Include(g => g.CurrentUser)
                .Include(g => g.Game)
                .Include(g => g.Order)
                .Include(g => g.Storefront)
                .FirstOrDefaultAsync(m => m.KeyId == id);
            if (gameKey == null)
            {
                return NotFound();
            }

            return View(gameKey);
        }

        // GET: Keys/Create
        public IActionResult Create()
        {
            ViewData["CurrentUserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["StorefrontId"] = new SelectList(_context.Storefronts, "StorefrontId", "StorefrontId");
            return View();
        }

        // POST: Keys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KeyId,LicenseKey,PurchaseDate,IsActive,GameId,StorefrontId,CurrentUserId,OrderId,RevocationReason")] GameKey gameKey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameKey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrentUserId"] = new SelectList(_context.Users, "UserId", "UserId", gameKey.CurrentUserId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameKey.GameId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameKey.OrderId);
            ViewData["StorefrontId"] = new SelectList(_context.Storefronts, "StorefrontId", "StorefrontId", gameKey.StorefrontId);
            return View(gameKey);
        }

        // GET: Keys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameKey = await _context.GameKeys.FindAsync(id);
            if (gameKey == null)
            {
                return NotFound();
            }
            ViewData["CurrentUserId"] = new SelectList(_context.Users, "UserId", "UserId", gameKey.CurrentUserId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameKey.GameId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameKey.OrderId);
            ViewData["StorefrontId"] = new SelectList(_context.Storefronts, "StorefrontId", "StorefrontId", gameKey.StorefrontId);
            return View(gameKey);
        }

        // POST: Keys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KeyId,LicenseKey,PurchaseDate,IsActive,GameId,StorefrontId,CurrentUserId,OrderId,RevocationReason")] GameKey gameKey)
        {
            if (id != gameKey.KeyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameKey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameKeyExists(gameKey.KeyId))
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
            ViewData["CurrentUserId"] = new SelectList(_context.Users, "UserId", "UserId", gameKey.CurrentUserId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", gameKey.GameId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", gameKey.OrderId);
            ViewData["StorefrontId"] = new SelectList(_context.Storefronts, "StorefrontId", "StorefrontId", gameKey.StorefrontId);
            return View(gameKey);
        }

        // GET: Keys/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameKey = await _context.GameKeys
                .Include(g => g.CurrentUser)
                .Include(g => g.Game)
                .Include(g => g.Order)
                .Include(g => g.Storefront)
                .FirstOrDefaultAsync(m => m.KeyId == id);
            if (gameKey == null)
            {
                return NotFound();
            }

            return View(gameKey);
        }

        // POST: Keys/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameKey = await _context.GameKeys.FindAsync(id);
            if (gameKey != null)
            {
                _context.GameKeys.Remove(gameKey);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameKeyExists(int id)
        {
            return _context.GameKeys.Any(e => e.KeyId == id);
        }
    }
}
