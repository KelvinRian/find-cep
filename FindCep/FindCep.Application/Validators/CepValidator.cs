using System.Text.RegularExpressions;

namespace FindCep.Application.Validators
{
    public static class CepValidator
    {
        private static readonly string _cepRegex = @"^(\d{8}|\d{5}-\d{3})$";

        public static bool IsValid(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return false;

            return Regex.IsMatch(cep, _cepRegex);
        }
    }
}
