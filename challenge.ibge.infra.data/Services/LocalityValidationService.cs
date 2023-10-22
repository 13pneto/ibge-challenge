using System.Text.RegularExpressions;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Services.Interfaces;

namespace challenge.ibge.infra.data.Services;

public class LocalityValidationService : ILocalityValidationService
{
    public async Task<LocalityValidationResultDto> ValidateCanImport(LocalityDto localityDto)
    {
        var isValidUf = localityDto.UF.Length == 2;
        var isValidCity = Regex.IsMatch(localityDto.City, "^([-\"' \\p{L}]+?)+$");
        var isValidIbgeCode = Regex.IsMatch(localityDto.Code.ToString(), "^[0-9]{7}$");

        LocalityValidationResultDto localityValidationResultDto = new()
        {
            Locality = localityDto,
            InvalidFields = new()
        };

        if (isValidUf == false)
        {
            localityValidationResultDto.InvalidFields.Add(nameof(localityDto.UF));
        }
        if (isValidCity == false)
        {
            localityValidationResultDto.InvalidFields.Add(nameof(localityDto.City));            
        }
        if (isValidIbgeCode == false)
        {
            localityValidationResultDto.InvalidFields.Add(nameof(localityDto.Code));            
        }

        localityValidationResultDto.IsValid =
            isValidUf &&
            isValidCity &&
            isValidIbgeCode;

        return localityValidationResultDto;
    }
}

