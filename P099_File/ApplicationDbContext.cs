using Microsoft.EntityFrameworkCore;
using P099_File.Models;

namespace P099_File.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ChangeRecord> ChangeRecords { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChangeRecord>()
                .Property(c => c.EntityName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ChangeRecord>()
                .Property(c => c.ChangeTime)
                .HasDefaultValueSql("GETDATE()"); // SQL Server default timestamp

            // Any additional model configuration can go here
        }
    }
}
