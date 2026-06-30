using FindCep.Application.Validators;

namespace FindCep.Tests.Application.Validators
{
    public class CepValidatorTests
    {
        [Fact]
        public void IsValid_ShouldReturn_True_For_EightNumericDigits()
        {
            var cep = "12345678";

            var result = CepValidator.IsValid(cep);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturn_True_For_EightNumericDigits_PlusHyphen()
        {
            var cep = "12345-678";

            var result = CepValidator.IsValid(cep);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturn_False_For_MisplacedHyphen()
        {
            var cep = "12345678-";

            var result = CepValidator.IsValid(cep);

            Assert.False(result);
        }

        [Theory]
        [InlineData("12345-abc")]
        [InlineData("12345=678")]
        public void IsValid_ShouldReturn_False_For_InvalidCharacters(string cep)
        {
            var result = CepValidator.IsValid(cep);

            Assert.False(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123456789")]
        [InlineData("1234567890")]
        public void IsValid_ShouldReturn_False_For_WrongQuantityOfNumericDigits(string cep)
        {
            var result = CepValidator.IsValid(cep);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturn_False_For_EmptyOrString()
        {
            var cep = "";

            var result = CepValidator.IsValid(cep);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturn_False_For_NullString()
        {
            var result = CepValidator.IsValid(null);

            Assert.False(result);
        }
    }
}
