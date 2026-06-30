using FindCep.Application.Common;
using FindCep.Application.Dtos;

namespace FindCep.Application.UseCases
{
    public interface IGetAddressUseCase
    {
        Task<Result<ViaCepResponseDto>> ExecuteAsync(string cep);
    }
}
