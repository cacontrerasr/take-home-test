using Fundo.Applications.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration
{
    public class LoanManagementControllerTests : IClassFixture<LoanApiFactory>
    {
        private readonly HttpClient _client;

        public LoanManagementControllerTests(LoanApiFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Add("X-Api-Key", "local-dev-key");
        }

        [Fact]
        public async Task GetLoans_ShouldReturnSeededLoans()
        {
            var response = await _client.GetAsync("/loans");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var loans = await response.Content.ReadFromJsonAsync<List<LoanResponse>>();
            Assert.NotNull(loans);
            Assert.NotEmpty(loans);
        }

        [Fact]
        public async Task GetLoanById_ShouldReturnLoan()
        {
            var response = await _client.GetAsync("/loans/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var loan = await response.Content.ReadFromJsonAsync<LoanResponse>();
            Assert.NotNull(loan);
            Assert.Equal(1, loan!.Id);
        }

        [Fact]
        public async Task CreateLoanAndApplyPayment_ShouldUpdateBalance()
        {
            var createResponse = await _client.PostAsJsonAsync("/loans", new LoanCreateRequest
            {
                Amount = 1000m,
                ApplicantName = "Test Applicant"
            });

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdLoan = await createResponse.Content.ReadFromJsonAsync<LoanResponse>();
            Assert.NotNull(createdLoan);

            var paymentResponse = await _client.PostAsJsonAsync($"/loans/{createdLoan!.Id}/payment", new LoanPaymentRequest
            {
                Amount = 1000m
            });

            Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);

            var paidLoan = await paymentResponse.Content.ReadFromJsonAsync<LoanResponse>();
            Assert.NotNull(paidLoan);
            Assert.Equal(0m, paidLoan!.CurrentBalance);
            Assert.Equal("paid", paidLoan.Status);
        }
    }
}
