using FindCep.Application.Common;
using FindCep.Application.Dtos;
using FindCep.Application.Enums;
using FindCep.Application.Services;
using FindCep.Application.Validators;

namespace FindCep.Application.UseCases
{
    public class GetAddressUseCase : IGetAddressUseCase
    {
        private IViaCepService _viaCepService;

        public GetAddressUseCase(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        public async Task<Result<ViaCepResponseDto>> ExecuteAsync(string cep)
        {
            var cepIsValid = CepValidator.IsValid(cep);
            
            if (cepIsValid)
                return await FindAddress(cep);
            else
                return Result<ViaCepResponseDto>.Failure(Error.InvalidCep, "O CEP deve estar no formato 00000000 ou 00000-000.");
        }

        private async Task<Result<ViaCepResponseDto>> FindAddress(string cep)
        {
            return await _viaCepService.GetAddressByCepAsync(cep);
        }
    }
}
