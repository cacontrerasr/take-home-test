using System.ComponentModel.DataAnnotations;

namespace Fundo.Applications.WebApi.Contracts
{
    public class LoanPaymentRequest
    {
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
