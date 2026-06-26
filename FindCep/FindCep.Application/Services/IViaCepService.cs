using FindCep.Application.Dtos;

namespace FindCep.Application.Services
{
    public interface IViaCepService
    {
        Task<CepDto> GetAddressByCepAsync(string cep);
    }
}
