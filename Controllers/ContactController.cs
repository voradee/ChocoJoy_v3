using Microsoft.AspNetCore.Mvc;
using ChocoJoy.Data;
using ChocoJoy.Models;

namespace ChocoJoy.Controllers;

public class ContactController : Controller
{
    private readonly AppDbContext _db;
    public ContactController(AppDbContext db) => _db = db;

    [HttpGet] public IActionResult Index() => View();
    [HttpPost] public IActionResult Index(ContactMessage model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.ContactMessages.Add(model);
        _db.SaveChanges();
        ViewBag.Message = "Thank you — we received your message.";
        return View();
    }
}
