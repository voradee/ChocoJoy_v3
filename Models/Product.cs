using System.ComponentModel.DataAnnotations;

namespace ChocoJoy.Models;

public class Product
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LongDescription { get; set; }
    [Range(0, double.MaxValue)] public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
