using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace FindCep.Infrastructure.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<CepDto>> GetAddressByCepAsync(string cep)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{cep}/json/");

                if (!response.IsSuccessStatusCode)
                {
                    return Result<CepDto>.Failure(
                        Error.ExternalServiceUnavailable,
                        "Não foi possível consultar o ViaCEP.");
                }
                
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);

                var dto = await response.Content.ReadFromJsonAsync<CepDto>();

                if (dto?.Erro == "true")
                {
                    return Result<CepDto>.Failure(
                        Error.CepNotFound,
                        "CEP não encontrado.");
                }

                return Result<CepDto>.Success(dto);
            }
            catch (HttpRequestException)
            {
                return Result<CepDto>.Failure(
                    Error.ExternalServiceUnavailable,
                    "Não foi possível conectar ao ViaCEP.");
            }
            catch (TaskCanceledException)
            {
                return Result<CepDto>.Failure(
                    Error.Timeout,
                    "Tempo limite da consulta ao ViaCEP excedido.");
            }
            catch (JsonException)
            {
                return Result<CepDto>.Failure(
                    Error.InvalidResponse,
                    "Resposta inválida recebida do ViaCEP.");
            }
        }
    }
}
