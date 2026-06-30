using FindCep.Application.Enums;

namespace FindCep.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public Error? Error { get; }
        public string? Message { get; }

        private Result(bool isSuccess, T? data, Error? error, string? message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            Message = message;
        }

        public static Result<T> Success(T data)
            => new(true, data, null, null);

        public static Result<T> Failure(Error? error, string message)
            => new(false, default, error, message);
    }
}
