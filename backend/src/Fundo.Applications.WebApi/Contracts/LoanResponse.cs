using System;

namespace Fundo.Applications.WebApi.Contracts
{
    public class LoanResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}
