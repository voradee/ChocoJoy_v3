using Microsoft.EntityFrameworkCore;
using ChocoJoy.Data;
using ChocoJoy.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// ✅ Database file setup
var dbPath = Path.Combine(AppContext.BaseDirectory, "AppData", "chocojoy_v3.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));

// ✅ Cookie authentication setup
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// ✅ Database seeding (with local image paths)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(new[]
        {
            new Product {
                Name = "Classic Dark Chocolate",
                Description = "Rich 70% cocoa dark chocolate bar.",
                LongDescription = "An intense dark chocolate made from single-origin cocoa beans. Notes of blackberry and espresso. Ingredients: Cocoa mass, sugar, cocoa butter, soy lecithin.",
                Price = 6.99m,
                ImageUrl = "/images/products/1.jpg"
            },
            new Product {
                Name = "Hazelnut Crunch",
                Description = "Milk chocolate with roasted hazelnuts.",
                LongDescription = "Creamy milk chocolate studded with crunchy roasted hazelnuts. Perfect balance of sweet and nutty.",
                Price = 5.99m,
                ImageUrl = "/images/products/2.jpg"
            },
            new Product {
                Name = "Salted Caramel",
                Description = "Smooth caramel with sea salt center.",
                LongDescription = "Soft caramel center wrapped in smooth milk chocolate, finished with a sprinkle of sea salt for contrast.",
                Price = 7.49m,
                ImageUrl = "/images/products/3.jpg"
            },
            new Product {
                Name = "Berry Truffle",
                Description = "White chocolate truffle with berry filling.",
                LongDescription = "Delicate white chocolate truffle filled with a tart berry ganache, handmade in small batches.",
                Price = 6.49m,
                ImageUrl = "/images/products/4.jpg"
            },
            new Product {
                Name = "Mint Crisp",
                Description = "Refreshing mint chocolate with crisp bits.",
                LongDescription = "Cool peppermint ganache with crisped rice for a satisfying crunch.",
                Price = 5.49m,
                ImageUrl = "/images/products/5.jpg"
            },
            new Product {
                Name = "Luxury Collection",
                Description = "Handpicked luxury chocolate assortment.",
                LongDescription = "A curated selection of our finest truffles and bars, ideal for gifting.",
                Price = 24.99m,
                ImageUrl = "/images/products/6.jpg"
            }
        });
        db.SaveChanges();
    }
}

// ✅ Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles(); // allows loading local images (wwwroot/images)
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
