using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class BudgetMember
    {
        public int BudgetId { get; set; }
        public Budget Budget { get; set; } = null;

        public int UserId { get; set; }
        public User User { get; set; } = null;

        public BudgetRole Role { get; set; }

        [Precision(18, 2)]
        public decimal? ContributionAmount { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
