using FindCep.Application.Common;
using FindCep.Application.Enums;

namespace FindCep.Tests.Application.Common
{
    public class ResultTests
    {
        [Fact]
        public void Success_ShouldReturn_SuccessResult()
        {
            // Arrange
            var data = "Test Data";

            // Act
            var result = Result<string>.Success(data);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(data, result.Data);
            Assert.Null(result.Error);
            Assert.Null(result.Message);
        }

        [Fact]
        public void Failure_ShouldReturn_FailureResult()
        {
            // Arrange
            var error = Error.InvalidResponse;
            var message = "An error occurred";

            // Act
            var result = Result<string>.Failure(error, message);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(error, result.Error);
            Assert.Equal(message, result.Message);
        }
    }
}
