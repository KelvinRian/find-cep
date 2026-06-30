using FindCep.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindCep.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CepsController : ControllerBase
    {
        private IGetAddressUseCase _getAddressUseCase;

        public CepsController(IGetAddressUseCase getAddressUseCase)
        {
            _getAddressUseCase = getAddressUseCase;
        }

        [HttpGet("{cep}")]
        public async Task<IActionResult> Get([FromRoute] string cep)
        {
            var cepDto = await _getAddressUseCase.ExecuteAsync(cep);
            return Ok(cepDto);
        }
    }
}
