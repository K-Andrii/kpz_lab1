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
    public class StorefrontsController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public StorefrontsController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        // GET: Storefronts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Storefronts.ToListAsync());
        }

        // GET: Storefronts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storefront = await _context.Storefronts
                .FirstOrDefaultAsync(m => m.StorefrontId == id);
            if (storefront == null)
            {
                return NotFound();
            }

            return View(storefront);
        }

        // GET: Storefronts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Storefronts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StorefrontId,Name")] Storefront storefront)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storefront);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storefront);
        }

        // GET: Storefronts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storefront = await _context.Storefronts.FindAsync(id);
            if (storefront == null)
            {
                return NotFound();
            }
            return View(storefront);
        }

        // POST: Storefronts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StorefrontId,Name")] Storefront storefront)
        {
            if (id != storefront.StorefrontId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storefront);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorefrontExists(storefront.StorefrontId))
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
            return View(storefront);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storefront = await _context.Storefronts
                .FirstOrDefaultAsync(m => m.StorefrontId == id);
            if (storefront == null)
            {
                return NotFound();
            }

            return View(storefront);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storefront = await _context.Storefronts.FindAsync(id);
            if (storefront != null)
            {
                _context.Storefronts.Remove(storefront);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorefrontExists(int id)
        {
            return _context.Storefronts.Any(e => e.StorefrontId == id);
        }
    }
}
