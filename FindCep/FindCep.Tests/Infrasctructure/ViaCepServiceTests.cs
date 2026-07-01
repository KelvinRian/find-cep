using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace FindCep.Tests.Infrasctructure
{
    public class ViaCepServiceTests
    {
        [Fact]
        public async Task GetAsync_ShouldReturn_SuccessResult_From_ViaCepApi()
        {
            // Arrange
            var cep = "01001000";

            var viaCepResponse = new ViaCepResponseDto()
            {
                Cep = cep,
                Logradouro = "Praça da Sé",
                Complemento = "lado ímpar",
                Unidade = "",
                Bairro = "Sé",
                Localidade = "São Paulo",
                Uf = "SP",
                Estado = "São Paulo",
                Regiao = "Sudeste",
                Ibge = "3550308",
                Gia = "1004",
                Ddd = "11",
                Siafi = "7107",
                Erro = null
            };

            var jsonResponse = JsonSerializer.Serialize(viaCepResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            var result = await service.GetAsync(cep);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(cep, result.Data.Data.Cep);
            Assert.Equal(DataOrigin.ViaCepApi.ToString(), result.Data.DataOrigin);

            bool isCached = memoryCache.TryGetValue(cep, out var cachedValue);
            Assert.True(isCached);
            Assert.NotNull(cachedValue);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_SuccessResult_From_Cache()
        {
            // Arrange
            var cep = "01001000";

            var viaCepResponse = new ViaCepResponseDto()
            {
                Cep = cep,
                Logradouro = "Praça da Sé",
                Complemento = "lado ímpar",
                Unidade = "",
                Bairro = "Sé",
                Localidade = "São Paulo",
                Uf = "SP",
                Estado = "São Paulo",
                Regiao = "Sudeste",
                Ibge = "3550308",
                Gia = "1004",
                Ddd = "11",
                Siafi = "7107",
                Erro = null
            };

            var jsonResponse = JsonSerializer.Serialize(viaCepResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            await service.GetAsync(cep);
            var result = await service.GetAsync(cep);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(cep, result.Data.Data.Cep);
            Assert.Equal(DataOrigin.Cache.ToString(), result.Data.DataOrigin);

            bool isCached = memoryCache.TryGetValue(cep, out var cachedValue);
            Assert.True(isCached);
            Assert.NotNull(cachedValue);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_FailureResult_When_CepNotFound()
        {
            // Arrange
            var cep = "01001000";

            var viaCepResponse = new ViaCepResponseDto()
            {
                Erro = "true"
            };

            var jsonResponse = JsonSerializer.Serialize(viaCepResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            var result = await service.GetAsync(cep);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(Error.CepNotFound, result.Error);
            Assert.Equal("CEP não encontrado.", result.Message);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_FailureResult_When_HttpRequestException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Erro de conexão"));

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            var cep = "01001000";
            var result = await service.GetAsync(cep);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(Error.ExternalServiceUnavailable, result.Error);
            Assert.Equal("Não foi possível conectar ao ViaCEP.", result.Message);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_FailureResult_When_TaskCanceledException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Timeout"));

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            var cep = "01001000";
            var result = await service.GetAsync(cep);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(Error.Timeout, result.Error);
            Assert.Equal("Tempo limite da consulta ao ViaCEP excedido.", result.Message);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_FailureResult_When_JsonException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{{{ json inválido }}}")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ViaCepService(httpClient, memoryCache);

            // Act
            var cep = "01001000";
            var result = await service.GetAsync(cep);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(Error.InvalidResponse, result.Error);
            Assert.Equal("Resposta inválida recebida do ViaCEP.", result.Message);
        }
    }
}
