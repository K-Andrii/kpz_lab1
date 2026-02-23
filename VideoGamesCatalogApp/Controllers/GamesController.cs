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
    public class GamesController : Controller
    {
        private readonly VideoGamesCatalogContext _context;

        public GamesController(VideoGamesCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrPriceSortParm"] = sortOrder == "CurrPrice" ? "curr_price_desc" : "CurrPrice";
            ViewData["ScoreSortParm"] = sortOrder == "Score" ? "score_desc" : "Score";
            ViewData["CurrentFilter"] = searchString;

            var games = _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Title.Contains(searchString) || s.Developer.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc": games = games.OrderByDescending(s => s.Title); break;
                case "Price": games = games.OrderBy(s => s.BasePrice); break;
                case "price_desc": games = games.OrderByDescending(s => s.BasePrice); break;
                case "Date": games = games.OrderBy(s => s.ReleaseDate); break;
                case "date_desc": games = games.OrderByDescending(s => s.ReleaseDate); break;
                case "CurrPrice": games = games.OrderBy(s => s.CurrentPrice); break;
                case "curr_price_desc": games = games.OrderByDescending(s => s.CurrentPrice); break;
                case "Score": games = games.OrderBy(s => s.MetaScore); break;
                case "score_desc": games = games.OrderByDescending(s => s.MetaScore); break;
                default: games = games.OrderBy(s => s.Title); break;
            }

            return View(await games.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperId");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create([Bind("GameId,Title,Description,ReleaseDate,AgeRating,BasePrice,CurrentPrice,MetaScore,TrailerUrl,DeveloperId,PublisherId")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperId", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherId", game.PublisherId);
            return View(game);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperId", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherId", game.PublisherId);
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Title,Description,ReleaseDate,AgeRating,BasePrice,CurrentPrice,MetaScore,TrailerUrl,DeveloperId,PublisherId")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperId", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherId", game.PublisherId);
            return View(game);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var game = await _context.Games.FindAsync(id);
            var activeUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (game == null || activeUser == null) return NotFound();

            if (activeUser.WalletBalance < game.CurrentPrice)
            {
                TempData["Error"] = "Недостатньо коштів на гаманці!";
                return RedirectToAction(nameof(Index));
            }

            var gameKey = await _context.GameKeys
                .FirstOrDefaultAsync(k => k.GameId == id && k.CurrentUserId == null);

            if (gameKey == null)
            {
                TempData["Error"] = "На жаль, ключі для цієї гри закінчилися.";
                return RedirectToAction(nameof(Index));
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    activeUser.WalletBalance -= game.CurrentPrice;

                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        PaymentMethod = "Internal Wallet",
                        OrderStatus = "Completed",
                        TotalAmount = game.CurrentPrice,
                        TransactionId = Guid.NewGuid().ToString(),
                        UserId = activeUser.UserId
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    gameKey.CurrentUserId = activeUser.UserId;
                    gameKey.IsActive = true;
                    gameKey.PurchaseDate = DateTime.Now; 
                    gameKey.OrderId = order.OrderId;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["Success"] = $"Вітаємо! Ви придбали {game.Title}.";
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = "Сталася помилка при оформленні замовлення.";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
