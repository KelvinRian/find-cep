using FindCep.Application.Common;
using FindCep.Application.Dtos;

namespace FindCep.Application.Services
{
    public interface IViaCepService
    {
        /// <summary>
        /// Requests address information from the ViaCEP API based on the provided CEP and caches the result for five minutes.
        /// </summary>
        /// <param name="cep">CEP to be searched</param>
        /// <returns>Returns a Result object with cep data and data origin</returns>
        Task<Result<CepDto>> GetAsync(string cep);
    }
}
