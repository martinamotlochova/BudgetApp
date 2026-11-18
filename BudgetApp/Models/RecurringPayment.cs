using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models;

public class RecurringPayment
{
    public int Id { get; set; }

    public int BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public string? Note { get; set; }
    public int DayOfMonth { get; set; }        // 1–31
    public bool IsActive { get; set; } = true;

    // Last date a Transaction was generated – prevents creating it twice
    public DateOnly? LastGeneratedOn { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
