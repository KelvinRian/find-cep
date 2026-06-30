using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text.Json;

namespace FindCep.Infrastructure.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public ViaCepService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<Result<ViaCepResponseDto>> GetAddressByCepAsync(string cep)
        {
            if (_cache.TryGetValue(cep, out ViaCepResponseDto? cachedCep))
            {
                cachedCep.DataOrigin = DataOrigin.Cache.ToString();
                _cache.Set(cep, cachedCep, TimeSpan.FromMinutes(1));
                return Result<ViaCepResponseDto>.Success(cachedCep);
            }

            try
            {
                var response = await _httpClient.GetAsync($"{cep}/json/");

                if (!response.IsSuccessStatusCode)
                {
                    return Result<ViaCepResponseDto>.Failure(
                        Error.ExternalServiceUnavailable,
                        "Não foi possível consultar o ViaCEP.");
                }

                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);

                var dto = await response.Content.ReadFromJsonAsync<ViaCepResponseDto>();

                if (dto != null)
                    dto.DataOrigin = DataOrigin.ViaCepApi.ToString();

                if (dto?.Erro == "true")
                {
                    return Result<ViaCepResponseDto>.Failure(
                        Error.CepNotFound,
                        "CEP não encontrado.");
                }

                _cache.Set(
                    cep,
                    dto,
                    TimeSpan.FromMinutes(1));

                return Result<ViaCepResponseDto>.Success(dto);
            }
            catch (HttpRequestException)
            {
                return Result<ViaCepResponseDto>.Failure(
                    Error.ExternalServiceUnavailable,
                    "Não foi possível conectar ao ViaCEP.");
            }
            catch (TaskCanceledException)
            {
                return Result<ViaCepResponseDto>.Failure(
                    Error.Timeout,
                    "Tempo limite da consulta ao ViaCEP excedido.");
            }
            catch (JsonException)
            {
                return Result<ViaCepResponseDto>.Failure(
                    Error.InvalidResponse,
                    "Resposta inválida recebida do ViaCEP.");
            }
        }
    }
}
