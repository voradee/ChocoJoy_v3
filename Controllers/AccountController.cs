using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ChocoJoy.Data;
using ChocoJoy.Models;

namespace ChocoJoy.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;
    public AccountController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Login(string? returnUrl) { ViewData["ReturnUrl"] = returnUrl; return View(); }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        // allow demo admin credentials
        if (username == "admin" && password == "12345")
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "Admin") };
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index","Products");
        }
        if (user != null && VerifyHash(password, user.PasswordHash))
        {
            var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index","Home");
        }
        ModelState.AddModelError(string.Empty, "Invalid credentials");
        return View();
    }

    [HttpGet] public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        if (_db.Users.Any(u => u.Username == username || u.Email == email))
        {
            ModelState.AddModelError(string.Empty, "Username or email already exists");
            return View();
        }
        var hash = HashPassword(password);
        var user = new User { Username = username, Email = email, PasswordHash = hash };
        _db.Users.Add(user);
        _db.SaveChanges();
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        return RedirectToAction("Index","Home");
    }

    public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); return RedirectToAction("Index","Home"); }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password + "|choco_salt"));
        return Convert.ToBase64String(bytes);
    }
    private static bool VerifyHash(string password, string hash) => HashPassword(password) == hash;
}
