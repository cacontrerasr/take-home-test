using Fundo.Applications.WebApi.Contracts;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("loans")]
    public class LoanManagementController : ControllerBase
    {
        private readonly LoanDbContext _context;

        public LoanManagementController(LoanDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<LoanResponse>>> GetAll()
        {
            var loans = await _context.Loans
                .AsNoTracking()
                .OrderByDescending(loan => loan.CreatedAtUtc)
                .Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    Amount = loan.Amount,
                    CurrentBalance = loan.CurrentBalance,
                    ApplicantName = loan.ApplicantName,
                    Status = loan.Status.ToString().ToLowerInvariant(),
                    CreatedAtUtc = loan.CreatedAtUtc
                })
                .ToListAsync();

            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LoanResponse>> GetById(int id)
        {
            var loan = await _context.Loans.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return Ok(new LoanResponse
            {
                Id = loan.Id,
                Amount = loan.Amount,
                CurrentBalance = loan.CurrentBalance,
                ApplicantName = loan.ApplicantName,
                Status = loan.Status.ToString().ToLowerInvariant(),
                CreatedAtUtc = loan.CreatedAtUtc
            });
        }

        [HttpPost]
        public async Task<ActionResult<LoanResponse>> Create([FromBody] LoanCreateRequest request)
        {
            var loan = new Loan
            {
                Amount = request.Amount,
                CurrentBalance = request.Amount,
                ApplicantName = request.ApplicantName,
                Status = LoanStatus.Active,
                CreatedAtUtc = System.DateTime.UtcNow
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            var response = new LoanResponse
            {
                Id = loan.Id,
                Amount = loan.Amount,
                CurrentBalance = loan.CurrentBalance,
                ApplicantName = loan.ApplicantName,
                Status = loan.Status.ToString().ToLowerInvariant(),
                CreatedAtUtc = loan.CreatedAtUtc
            };

            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, response);
        }

        [HttpPost("{id:int}/payment")]
        public async Task<ActionResult<LoanResponse>> ApplyPayment(int id, [FromBody] LoanPaymentRequest request)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(item => item.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            if (loan.Status == LoanStatus.Paid)
            {
                return BadRequest("Loan already paid.");
            }

            loan.CurrentBalance -= request.Amount;
            if (loan.CurrentBalance <= 0)
            {
                loan.CurrentBalance = 0;
                loan.Status = LoanStatus.Paid;
            }

            await _context.SaveChangesAsync();

            return Ok(new LoanResponse
            {
                Id = loan.Id,
                Amount = loan.Amount,
                CurrentBalance = loan.CurrentBalance,
                ApplicantName = loan.ApplicantName,
                Status = loan.Status.ToString().ToLowerInvariant(),
                CreatedAtUtc = loan.CreatedAtUtc
            });
        }
    }
}
