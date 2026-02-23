using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VideoGamesCatalogApp.Models;

public class AccountController : Controller
{
    private readonly VideoGamesCatalogContext _context;

    public AccountController(VideoGamesCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username.Trim() == username.Trim());

        if (user != null && user.PasswordHash.Trim() == password.Trim())
        {
            user.LastLoginDate = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username.Trim()),
            new Claim(ClaimTypes.Role, user.Role?.RoleName.Trim() ?? "User"),
            new Claim("UserId", user.UserId.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid username or password.";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username.Trim() == username.Trim()))
        {
            ViewBag.Error = "Цей логін уже зайнятий.";
            return View();
        }

        var newUser = new User
        {
            Username = username.Trim(),
            Email = email.Trim(),
            PasswordHash = password.Trim(),
            RoleId = 3,
            RegistrationDate = DateTime.Now,
            WalletBalance = 0,
            IsBlocked = false
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, newUser.Username),
        new Claim(ClaimTypes.Role, "User"),
        new Claim("UserId", newUser.UserId.ToString())
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }
}