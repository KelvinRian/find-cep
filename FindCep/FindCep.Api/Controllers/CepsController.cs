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
