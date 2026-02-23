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
    public class HardwareController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public HardwareController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        // GET: Hardware
        public async Task<IActionResult> Index()
        {
            var videoGamesCatalogContext = _context.HardwareComponents.Include(h => h.Type);
            return View(await videoGamesCatalogContext.ToListAsync());
        }

        // GET: Hardware/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponents
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.ComponentId == id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }

            return View(hardwareComponent);
        }

        // GET: Hardware/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.HardwareTypes, "TypeId", "TypeId");
            return View();
        }

        // POST: Hardware/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComponentId,Name,Manufacturer,BenchmarkScore,TypeId")] HardwareComponent hardwareComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hardwareComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.HardwareTypes, "TypeId", "TypeId", hardwareComponent.TypeId);
            return View(hardwareComponent);
        }

        // GET: Hardware/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponents.FindAsync(id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.HardwareTypes, "TypeId", "TypeId", hardwareComponent.TypeId);
            return View(hardwareComponent);
        }

        // POST: Hardware/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComponentId,Name,Manufacturer,BenchmarkScore,TypeId")] HardwareComponent hardwareComponent)
        {
            if (id != hardwareComponent.ComponentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hardwareComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HardwareComponentExists(hardwareComponent.ComponentId))
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
            ViewData["TypeId"] = new SelectList(_context.HardwareTypes, "TypeId", "TypeId", hardwareComponent.TypeId);
            return View(hardwareComponent);
        }

        // GET: Hardware/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponents
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.ComponentId == id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }

            return View(hardwareComponent);
        }

        // POST: Hardware/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hardwareComponent = await _context.HardwareComponents.FindAsync(id);
            if (hardwareComponent != null)
            {
                _context.HardwareComponents.Remove(hardwareComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HardwareComponentExists(int id)
        {
            return _context.HardwareComponents.Any(e => e.ComponentId == id);
        }
    }
}
