namespace FindCep.Api.Authentication
{
    public class ApiKeyAuthenticationOptions
    {
        public const string SectionName = "Authentication";

        public string ApiKey { get; init; } = string.Empty;
    }
}
