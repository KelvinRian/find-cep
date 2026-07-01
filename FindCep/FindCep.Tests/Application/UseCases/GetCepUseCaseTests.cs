using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.Services;
using FindCep.Application.UseCases;
using Moq;

namespace FindCep.Tests.Application.UseCases
{
    public class GetCepUseCaseTests
    {
        private readonly Mock<IViaCepService> _viaCepService;
        private readonly GetCepUseCase _getCepUseCase;

        public GetCepUseCaseTests()
        {
            _viaCepService = new Mock<IViaCepService>();
            _getCepUseCase = new GetCepUseCase(_viaCepService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturn_ServiceResponse_When_CepIsValid()
        {
            // Arrange
            var cep = "01001000";
            var viaCepResponse = new ViaCepResponseDto { Cep = cep };
            var cepDto = new CepDto(viaCepResponse, DataOrigin.ViaCepApi);
            
            var expectedResult = Result<CepDto>.Success(cepDto);
            _viaCepService
                .Setup(x => x.GetAsync(cep))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _getCepUseCase.ExecuteAsync(cep);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturn_Failure_When_CepIsNotValid()
        {
            // Arrange
            var invalidCep = "--------";

            // Act
            var result = await _getCepUseCase.ExecuteAsync(invalidCep);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(Error.InvalidCep, result.Error);
            Assert.Equal("O CEP deve estar no formato 00000000 ou 00000-000.", result.Message);
        }
    }
}
