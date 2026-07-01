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

        public async Task<Result<CepDto>> GetAsync(string cep)
        {
            var cepIsCached = _cache.TryGetValue(cep, out ViaCepResponseDto? cachedCep);

            if (cepIsCached)
                return ReturnCachedCep(cep, cachedCep);
            else
                return await TryGetFromViaCep(cep);
        }

        private Result<CepDto> ReturnCachedCep(string cep, ViaCepResponseDto? cachedCep)
        {
            _cache.Set(cep, cachedCep, TimeSpan.FromMinutes(5));

            var cepDto = new CepDto(cachedCep, DataOrigin.Cache);
            return Result<CepDto>.Success(cepDto);
        }

        private async Task<Result<CepDto>> TryGetFromViaCep(string cep)
        {
            try
            {
                return await GetFromViaCep(cep);
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

        private async Task<Result<CepDto>> GetFromViaCep(string cep)
        {
            var response = await _httpClient.GetAsync($"{cep}/json/");

            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadFromJsonAsync<ViaCepResponseDto>();

                var cepNotFound = responseDto?.Erro == "true";
                if (cepNotFound)
                {
                    return Result<CepDto>.Failure(
                        Error.CepNotFound,
                        "CEP não encontrado.");
                }

                _cache.Set(
                    cep,
                    responseDto,
                    TimeSpan.FromMinutes(5));

                var cepDto = new CepDto(responseDto, DataOrigin.ViaCepApi);
                return Result<CepDto>.Success(cepDto);
            }

            return Result<CepDto>.Failure(
                Error.ExternalServiceUnavailable,
                "Não foi possível consultar o ViaCEP.");
        }
    }
}
