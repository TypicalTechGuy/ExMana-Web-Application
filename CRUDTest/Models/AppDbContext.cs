using Microsoft.EntityFrameworkCore;

namespace CRUDTest.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
    }
}
