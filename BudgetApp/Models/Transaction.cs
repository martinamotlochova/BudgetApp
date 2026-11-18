using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int BudgetId { get; set; }
        public Budget Budget { get; set; } = null!;

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        public string? Note { get; set; }
        public DateOnly Date { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
