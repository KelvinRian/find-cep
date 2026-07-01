using FindCep.Application.Common;
using FindCep.Application.Dtos;

namespace FindCep.Application.UseCases
{
    public interface IGetCepUseCase
    {
        /// <summary>
        /// Executes the use case to retrieve address information based on the provided CEP (postal code).
        /// </summary>
        /// <param name="cep">CEP to be searched</param>
        /// <returns>Returns a Result object with cep data and data origin</returns>
        Task<Result<CepDto>> ExecuteAsync(string cep);
    }
}
