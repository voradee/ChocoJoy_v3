using Microsoft.AspNetCore.Mvc;
using ChocoJoy.Data;

namespace ChocoJoy.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;
    public IActionResult Index() => View(_db.Products.Take(3).ToList());
    public IActionResult About() => View();
    public IActionResult Contact() => View();
}
