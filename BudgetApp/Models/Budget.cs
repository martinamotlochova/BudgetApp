using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal MonthlyLimit { get; set; }

        public SplitMode SplitMode { get; set; } = SplitMode.Equal;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<BudgetMember> Members { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public List<RecurringPayment> RecurringPayments { get; set; } = new();
    }
}
