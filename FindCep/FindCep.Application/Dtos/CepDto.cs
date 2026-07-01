using FindCep.Application.Enums;

namespace FindCep.Application.Dtos
{
    public class CepDto
    {
        public ViaCepResponseDto Data { get; set; }
        public string Source { get; set; }

        public CepDto(ViaCepResponseDto data, Source source)
        {
            Data = data;
            Source = source.ToString();
        }
    }
}
