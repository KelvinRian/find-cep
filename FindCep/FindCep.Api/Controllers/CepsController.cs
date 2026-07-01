using FindCep.Application.Dtos;
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
        private IGetCepUseCase _getCepUseCase;

        public CepsController(IGetCepUseCase getAddressUseCase)
        {
            _getCepUseCase = getAddressUseCase;
        }

        [HttpGet("{cep}")]
        [ProducesResponseType(typeof(CepDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(typeof(string), StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string cep)
        {
            var cepDtoResult = await _getCepUseCase.ExecuteAsync(cep);

            if (!cepDtoResult.IsSuccess)
            {
                return cepDtoResult.Error switch
                {
                    Error.InvalidCep => BadRequest(cepDtoResult.Message),
                    Error.ExternalServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, cepDtoResult.Message),
                    Error.CepNotFound => NotFound(cepDtoResult.Message),
                    Error.Timeout => StatusCode(StatusCodes.Status504GatewayTimeout, cepDtoResult.Message),
                    Error.InvalidResponse => StatusCode(StatusCodes.Status502BadGateway, cepDtoResult.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }

            return Ok(cepDtoResult.Data);
        }
    }
}
