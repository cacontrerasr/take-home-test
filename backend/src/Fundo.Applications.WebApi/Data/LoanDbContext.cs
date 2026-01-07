using Fundo.Applications.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fundo.Applications.WebApi.Data
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
        {
        }

        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var statusConverter = new EnumToStringConverter<LoanStatus>();

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(loan => loan.Id);
                entity.Property(loan => loan.Amount)
                    .HasPrecision(12, 2)
                    .IsRequired();
                entity.Property(loan => loan.CurrentBalance)
                    .HasPrecision(12, 2)
                    .IsRequired();
                entity.Property(loan => loan.ApplicantName)
                    .HasMaxLength(200)
                    .IsRequired();
                entity.Property(loan => loan.Status)
                    .HasConversion(statusConverter)
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(loan => loan.CreatedAtUtc)
                    .IsRequired();
            });

            modelBuilder.Entity<Loan>().HasData(
                new Loan
                {
                    Id = 1,
                    Amount = 1500.00m,
                    CurrentBalance = 500.00m,
                    ApplicantName = "Maria Silva",
                    Status = LoanStatus.Active,
                    CreatedAtUtc = new System.DateTime(2024, 1, 10, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new Loan
                {
                    Id = 2,
                    Amount = 2500.00m,
                    CurrentBalance = 0.00m,
                    ApplicantName = "John Doe",
                    Status = LoanStatus.Paid,
                    CreatedAtUtc = new System.DateTime(2024, 2, 1, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new Loan
                {
                    Id = 3,
                    Amount = 3200.00m,
                    CurrentBalance = 1750.00m,
                    ApplicantName = "Aisha Khan",
                    Status = LoanStatus.Active,
                    CreatedAtUtc = new System.DateTime(2024, 3, 5, 0, 0, 0, System.DateTimeKind.Utc)
                }
            );
        }
    }
}
