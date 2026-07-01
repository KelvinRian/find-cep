using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.Services;
using FindCep.Application.Validators;

namespace FindCep.Application.UseCases
{
    public class GetCepUseCase : IGetCepUseCase
    {
        private IViaCepService _viaCepService;

        public GetCepUseCase(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        public async Task<Result<CepDto>> ExecuteAsync(string cep)
        {
            var cepIsValid = CepValidator.IsValid(cep);
            
            if (cepIsValid)
                return await _viaCepService.GetAsync(cep);
            else
                return Result<CepDto>.Failure(Error.InvalidCep, "O CEP deve estar no formato 00000000 ou 00000-000.");
        }
    }
}
