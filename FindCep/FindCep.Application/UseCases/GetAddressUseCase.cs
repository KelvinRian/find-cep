using FindCep.Application.Dtos;
using FindCep.Application.Services;

namespace FindCep.Application.UseCases
{
    public class GetAddressUseCase : IGetAddressUseCase
    {
        private IViaCepService _viaCepService;

        public GetAddressUseCase(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        public async Task<CepDto> ExecuteAsync(string cep)
        {
            var cepDto = await _viaCepService.GetAddressByCepAsync(cep);
            return cepDto;
        }
    }
}
