using Microsoft.EntityFrameworkCore;

namespace Project0
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=FirstDb;Trusted_Connection=True;");
            }
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}
