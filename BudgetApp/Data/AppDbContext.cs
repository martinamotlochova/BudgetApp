using BudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<BudgetMember> BudgetMembers { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Income> Incomes { get; set; } = null!;
        public DbSet<RecurringPayment> RecurringPayments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(t => t.TokenHash)
                .IsUnique();

            modelBuilder.Entity<BudgetMember>(e =>
            {
                // composite primary key: one user can be in one budget only once
                e.HasKey(m => new { m.BudgetId, m.UserId });

                // BudgetMember -> Budget (N:1), deleting a budget deletes its members
                e.HasOne(m => m.Budget)
                    .WithMany(b => b.Members)
                    .HasForeignKey(m => m.BudgetId)
                    .OnDelete(DeleteBehavior.Cascade);

                // BudgetMember -> User (N:1), deleting a user removes their memberships
                e.HasOne(m => m.User)
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // store enum as text ("Owner") instead of number (0)
                e.Property(m => m.Role).HasConversion<string>().HasMaxLength(20);

                // fast lookup of "my budgets"
                e.HasIndex(m => m.UserId);
            });

            modelBuilder.Entity<Budget>(e =>
            {
                // name is required and max 100 characters
                e.Property(b => b.Name).HasMaxLength(100).IsRequired();

                // store enum as text ("Equal") instead of number (0)
                e.Property(b => b.SplitMode).HasConversion<string>().HasMaxLength(20);
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                // Transaction -> Budget (N:1), deleting a budget deletes its transactions
                e.HasOne(t => t.Budget)
                    .WithMany(b => b.Transactions)
                    .HasForeignKey(t => t.BudgetId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Transaction -> User who created it (N:1)
                // Restrict: a user can't be deleted while they have transactions
                e.HasOne(t => t.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.Property(t => t.Note).HasMaxLength(200);

                // fast "transactions of this budget in this month"
                e.HasIndex(t => new { t.BudgetId, t.Date });
            });

            modelBuilder.Entity<Income>(e =>
            {
                // Income -> User (N:1), deleting a user deletes their incomes
                e.HasOne(i => i.User)
                    .WithMany()
                    .HasForeignKey(i => i.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.Property(i => i.Note).HasMaxLength(200);

                // fast "my incomes in this month"
                e.HasIndex(i => new { i.UserId, i.Date });
            });

            modelBuilder.Entity<RecurringPayment>(e =>
            {
                // RecurringPayment -> Budget (N:1), deleting a budget deletes its rules
                e.HasOne(r => r.Budget)
                    .WithMany(b => b.RecurringPayments)
                    .HasForeignKey(r => r.BudgetId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.Property(r => r.Note).HasMaxLength(200);

                // database rejects DayOfMonth outside 1–31
                e.ToTable(t => t.HasCheckConstraint(
                    "CK_RecurringPayment_DayOfMonth", "\"DayOfMonth\" BETWEEN 1 AND 31"));
            });
        }
    }
}
