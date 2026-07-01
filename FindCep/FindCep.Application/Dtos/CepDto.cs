using FindCep.Application.Enums;

namespace FindCep.Application.Dtos
{
    public class CepDto
    {
        public ViaCepResponseDto Data { get; set; }
        public string DataOrigin { get; set; }

        public CepDto(ViaCepResponseDto data, DataOrigin origin)
        {
            Data = data;
            DataOrigin = origin.ToString();
        }
    }
}
