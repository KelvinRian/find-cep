using FindCep.Application.Dtos;

namespace FindCep.Application.UseCases
{
    public interface IGetAddressUseCase
    {
        Task<CepDto> ExecuteAsync(string cep);
    }
}
