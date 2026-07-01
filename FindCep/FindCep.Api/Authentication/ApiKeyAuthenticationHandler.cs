using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FindCep.Api.Authentication
{
    public class ApiKeyAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApiKeyAuthenticationOptions _options;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> authenticationOptions,
            IOptions<ApiKeyAuthenticationOptions> apiKeyOptions,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(authenticationOptions, logger, encoder)
        {
            _options = apiKeyOptions.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("API Key não informada."));
            }

            if (!string.Equals(apiKey, _options.ApiKey))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("API Key inválida."));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "ApiClient")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(
                AuthenticateResult.Success(ticket));
        }
    }
}
