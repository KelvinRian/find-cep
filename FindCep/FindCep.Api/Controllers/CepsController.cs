using FindCep.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FindCep.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CepsController : ControllerBase
    {
        [HttpGet("{cep}")]
        public async Task<IActionResult> Get([FromRoute] string cep)
        {
            var cepDto = new CepDto
            {
                Cep = cep,
                Logradouro = "Rua Exemplo",
                Complemento = "Apto 101",
                Unidade = "Unidade 1",
                Bairro = "Bairro Central",
                Localidade = "Cidade Modelo",
                Uf = "SP",
                Estado = "São Paulo",
                Regiao = "Sudeste",
                Ibge = "1234567",
                Gia = "9876",
                Ddd = "11",
                Siafi = "1234"
            };

            return Ok(cepDto);
        }
    }
}
