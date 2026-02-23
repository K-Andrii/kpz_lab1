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
    public class RequirementsController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public RequirementsController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        // GET: Requirements
        public async Task<IActionResult> Index()
        {
            var videoGamesCatalogContext = _context.SystemRequirements.Include(s => s.Cpu).Include(s => s.Game).Include(s => s.Gpu);
            return View(await videoGamesCatalogContext.ToListAsync());
        }

        // GET: Requirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemRequirement = await _context.SystemRequirements
                .Include(s => s.Cpu)
                .Include(s => s.Game)
                .Include(s => s.Gpu)
                .FirstOrDefaultAsync(m => m.SysReqId == id);
            if (systemRequirement == null)
            {
                return NotFound();
            }

            return View(systemRequirement);
        }

        // GET: Requirements/Create
        public IActionResult Create()
        {
            ViewData["CpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["GpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId");
            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SysReqId,RequirementType,RamGb,StorageGb,Os,DirectXversion,GameId,CpuId,GpuId")] SystemRequirement systemRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.CpuId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", systemRequirement.GameId);
            ViewData["GpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.GpuId);
            return View(systemRequirement);
        }

        // GET: Requirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemRequirement = await _context.SystemRequirements.FindAsync(id);
            if (systemRequirement == null)
            {
                return NotFound();
            }
            ViewData["CpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.CpuId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", systemRequirement.GameId);
            ViewData["GpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.GpuId);
            return View(systemRequirement);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SysReqId,RequirementType,RamGb,StorageGb,Os,DirectXversion,GameId,CpuId,GpuId")] SystemRequirement systemRequirement)
        {
            if (id != systemRequirement.SysReqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemRequirementExists(systemRequirement.SysReqId))
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
            ViewData["CpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.CpuId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", systemRequirement.GameId);
            ViewData["GpuId"] = new SelectList(_context.HardwareComponents, "ComponentId", "ComponentId", systemRequirement.GpuId);
            return View(systemRequirement);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemRequirement = await _context.SystemRequirements
                .Include(s => s.Cpu)
                .Include(s => s.Game)
                .Include(s => s.Gpu)
                .FirstOrDefaultAsync(m => m.SysReqId == id);
            if (systemRequirement == null)
            {
                return NotFound();
            }

            return View(systemRequirement);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemRequirement = await _context.SystemRequirements.FindAsync(id);
            if (systemRequirement != null)
            {
                _context.SystemRequirements.Remove(systemRequirement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemRequirementExists(int id)
        {
            return _context.SystemRequirements.Any(e => e.SysReqId == id);
        }
    }
}
