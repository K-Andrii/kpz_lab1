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
    [Authorize]
    public class WalletLogsController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public WalletLogsController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logsQuery = _context.WalletLogs.AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (userIdClaim != null)
                {
                    int currentUserId = int.Parse(userIdClaim);
                    logsQuery = logsQuery.Where(w => w.UserId == currentUserId);
                }
            }

            return View(await logsQuery.ToListAsync());
        }

        // GET: WalletLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walletLog = await _context.WalletLogs
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (walletLog == null)
            {
                return NotFound();
            }

            return View(walletLog);
        }

        // GET: WalletLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WalletLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogId,UserId,OldBalance,NewBalance,ChangeDate")] WalletLog walletLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(walletLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(walletLog);
        }

        // GET: WalletLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walletLog = await _context.WalletLogs.FindAsync(id);
            if (walletLog == null)
            {
                return NotFound();
            }
            return View(walletLog);
        }

        // POST: WalletLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogId,UserId,OldBalance,NewBalance,ChangeDate")] WalletLog walletLog)
        {
            if (id != walletLog.LogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(walletLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletLogExists(walletLog.LogId))
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
            return View(walletLog);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walletLog = await _context.WalletLogs
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (walletLog == null)
            {
                return NotFound();
            }

            return View(walletLog);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var walletLog = await _context.WalletLogs.FindAsync(id);
            if (walletLog != null)
            {
                _context.WalletLogs.Remove(walletLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletLogExists(int id)
        {
            return _context.WalletLogs.Any(e => e.LogId == id);
        }
    }
}
