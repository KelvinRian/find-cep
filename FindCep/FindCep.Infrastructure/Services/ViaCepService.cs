using FindCep.Application.Dtos;
using FindCep.Application.Services;
using System.Net.Http.Json;

namespace FindCep.Infrastructure.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CepDto> GetAddressByCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"{cep}/json/");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CepDto>();

            if (result?.Erro == true)
                return new CepDto();

            return result ?? new CepDto();
        }
    }
}
