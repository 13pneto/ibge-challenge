using System.Text.RegularExpressions;
using challenge.ibge.core.Dtos;
using challenge.ibge.core.Services.Interfaces;

namespace challenge.ibge.core.Services;

public class LocalityValidationService : ILocalityValidationService
{
    public async Task<LocalityValidationResultDto> Validate(LocalityDto localityDto)
    {
        var isValidUf = localityDto.UF?.Length == 2;
        var isValidCity = Regex.IsMatch(localityDto.City ?? "", "^(?=\\p{L}{2,})([-\\\"' \\p{L}]+?)+$");
        var isValidIbgeCode = Regex.IsMatch(localityDto.IbgeCode.ToString(), "^[0-9]{7}$");

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
            localityValidationResultDto.InvalidFields.Add(nameof(localityDto.IbgeCode));            
        }

        localityValidationResultDto.IsValid =
            isValidUf &&
            isValidCity &&
            isValidIbgeCode;

        return localityValidationResultDto;
    }
}

