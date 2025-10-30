using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChocoJoy.Data;
using ChocoJoy.Models;

namespace ChocoJoy.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    public ProductsController(AppDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    public IActionResult Index() => View(_db.Products.ToList());
    public IActionResult Details(int id) { var p=_db.Products.Find(id); if(p==null) return NotFound(); return View(p); }

    [Authorize] public IActionResult Create() => View(new Product());
    [HttpPost][Authorize] public IActionResult Create(Product product, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(product);
        if (image!=null) {
            var uploads = Path.Combine(_env.WebRootPath, "images","products"); Directory.CreateDirectory(uploads);
            var fn = Guid.NewGuid()+Path.GetExtension(image.FileName);
            using var fs = new FileStream(Path.Combine(uploads, fn), FileMode.Create); image.CopyTo(fs);
            product.ImageUrl = "/images/products/"+fn;
        }
        _db.Products.Add(product); _db.SaveChanges(); return RedirectToAction(nameof(Index));
    }

    [Authorize] public IActionResult Edit(int id) { var p=_db.Products.Find(id); if(p==null) return NotFound(); return View(p); }
    [HttpPost][Authorize] public IActionResult Edit(Product product, IFormFile? image) {
        if (!ModelState.IsValid) return View(product);
        if (image!=null) {
            var uploads = Path.Combine(_env.WebRootPath, "images","products"); Directory.CreateDirectory(uploads);
            var fn = Guid.NewGuid()+Path.GetExtension(image.FileName);
            using var fs = new FileStream(Path.Combine(uploads, fn), FileMode.Create); image.CopyTo(fs);
            product.ImageUrl = "/images/products/"+fn;
        }
        _db.Products.Update(product); _db.SaveChanges(); return RedirectToAction(nameof(Index));
    }

    [Authorize][HttpPost] public IActionResult Delete(int id) { var p=_db.Products.Find(id); if(p!=null) {_db.Products.Remove(p); _db.SaveChanges();} return RedirectToAction(nameof(Index)); }
}
