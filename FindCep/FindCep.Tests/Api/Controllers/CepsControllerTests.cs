using FindCep.Api.Controllers;
using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;

namespace FindCep.Tests.Api.Controllers
{
    public class CepsControllerTests
    {
        private readonly Mock<IGetCepUseCase> _getCepUseCaseMock;
        private readonly CepsController _controller;

        public CepsControllerTests()
        {
            _getCepUseCaseMock = new Mock<IGetCepUseCase>();
            _controller = new CepsController(_getCepUseCaseMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Ok_When_UseCaseReturnsSuccess()
        {
            // Arrange
            var cep = "01001000";
            var viaCepResponse = new ViaCepResponseDto { Cep = cep };
            var cepDto = new CepDto(viaCepResponse, DataOrigin.ViaCepApi);
            var resultDto = Result<CepDto>.Success(cepDto);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(resultDto);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(cepDto, okResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_BadRequest_When_InvalidCep()
        {
            // Arrange
            var cep = "0000000000000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(Error.InvalidCep, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Status503_When_ExternalServiceUnavailable()
        {
            // Arrange
            var cep = "01001000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(Error.ExternalServiceUnavailable, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal(503, objectResult.StatusCode);
            Assert.Equal(errorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotFound_When_CepNotFound()
        {
            // Arrange
            var cep = "00000000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(Error.CepNotFound, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(errorMessage, notFoundResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Status504_When_Timeout()
        {
            // Arrange
            var cep = "00000000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(Error.Timeout, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal(504, objectResult.StatusCode);
            Assert.Equal(errorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Status502_When_InvalidResponse()
        {
            // Arrange
            var cep = "00000000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(Error.InvalidResponse, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal(502, objectResult.StatusCode);
            Assert.Equal(errorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Status500_When_UnmappedError()
        {
            // Arrange
            var cep = "00000000";
            var errorMessage = "messsage";
            var result = Result<CepDto>.Failure(null, errorMessage);

            _getCepUseCaseMock
                .Setup(x => x.ExecuteAsync(cep))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAsync(cep);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
