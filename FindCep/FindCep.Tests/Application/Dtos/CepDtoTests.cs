using FindCep.Application.Dtos;
using FindCep.Application.Enums;

namespace FindCep.Tests.Application.Dtos
{
    public class CepDtoTests
    {
        [Fact]
        public void CepDto_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var data = new ViaCepResponseDto();
            var source = Source.ViaCepApi;

            // Act
            var cepDto = new CepDto(data, source);

            // Assert
            Assert.Equal(data, cepDto.Data);
            Assert.Equal(source.ToString(), cepDto.Source);
        }
    }
}
