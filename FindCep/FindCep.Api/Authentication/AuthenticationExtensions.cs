using Microsoft.AspNetCore.Authentication;

namespace FindCep.Api.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddApiKeyAuthentication(
            this IServiceCollection services)
        {
            services
                .AddAuthentication("ApiKey")
                .AddScheme<AuthenticationSchemeOptions,
                    ApiKeyAuthenticationHandler>(
                    "ApiKey",
                    _ => { });

            services.AddAuthorization();

            return services;
        }
    }
}
