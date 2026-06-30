using FindCep.Application.Enums;
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
            var cepDtoResult = await _getAddressUseCase.ExecuteAsync(cep);

            if (!cepDtoResult.IsSuccess)
            {
                return cepDtoResult.Error switch
                {
                    Error.InvalidCep => BadRequest(cepDtoResult.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }

            return Ok(cepDtoResult.Data);
        }
    }
}
