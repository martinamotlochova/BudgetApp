using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models;

public class Income
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public string? Note { get; set; }
    public DateOnly Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
