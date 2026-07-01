using FindCep.Application.Common;
using FindCep.Application.Dtos;

namespace FindCep.Application.Services
{
    public interface IViaCepService
    {
        Task<Result<CepDto>> GetAddressByCepAsync(string cep);
    }
}
