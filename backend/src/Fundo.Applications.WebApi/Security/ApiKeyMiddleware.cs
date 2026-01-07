using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Security
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiKeyOptions _options;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeyOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(_options.Value))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var apiKey) || apiKey != _options.Value)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
