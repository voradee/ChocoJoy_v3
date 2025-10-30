using System.ComponentModel.DataAnnotations;

namespace ChocoJoy.Models;

public class ContactMessage
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    [Required] public string Subject { get; set; } = string.Empty;
    [Required] public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
