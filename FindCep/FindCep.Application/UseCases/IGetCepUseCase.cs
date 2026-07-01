using FindCep.Application.Common;
using FindCep.Application.Dtos;

namespace FindCep.Application.UseCases
{
    public interface IGetCepUseCase
    {
        Task<Result<CepDto>> ExecuteAsync(string cep);
    }
}
