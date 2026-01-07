using Fundo.Applications.WebApi.Models;
using System;
using System.Linq;

namespace Fundo.Applications.WebApi.Data
{
    public static class LoanDbInitializer
    {
        public static void Initialize(LoanDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Loans.Any())
            {
                return;
            }

            context.Loans.AddRange(
                new Loan
                {
                    Amount = 1500.00m,
                    CurrentBalance = 500.00m,
                    ApplicantName = "Maria Silva",
                    Status = LoanStatus.Active,
                    CreatedAtUtc = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Loan
                {
                    Amount = 2500.00m,
                    CurrentBalance = 0.00m,
                    ApplicantName = "John Doe",
                    Status = LoanStatus.Paid,
                    CreatedAtUtc = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Loan
                {
                    Amount = 3200.00m,
                    CurrentBalance = 1750.00m,
                    ApplicantName = "Aisha Khan",
                    Status = LoanStatus.Active,
                    CreatedAtUtc = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            context.SaveChanges();
        }
    }
}
