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

        /// <summary>
        /// Retrieves address information based on the provided CEP.
        /// </summary>
        /// <param name="cep">CEP to be searched</param>
        /// <returns>Returns address data for the provided cep.</returns>
        [HttpGet("{cep}")]
        [ProducesResponseType(typeof(CepDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(typeof(string), StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync([FromRoute] string cep)
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
