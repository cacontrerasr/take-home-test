using System.ComponentModel.DataAnnotations;

namespace Fundo.Applications.WebApi.Contracts
{
    public class LoanCreateRequest
    {
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(200)]
        public string ApplicantName { get; set; } = string.Empty;
    }
}
