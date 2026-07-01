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
            var dataOrigin = DataOrigin.ViaCepApi;

            // Act
            var cepDto = new CepDto(data, dataOrigin);

            // Assert
            Assert.Equal(data, cepDto.Data);
            Assert.Equal(dataOrigin.ToString(), cepDto.DataOrigin);
        }
    }
}
