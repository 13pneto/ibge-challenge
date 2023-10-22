using challenge.ibge.core.Dtos;

namespace challenge.ibge.core.Services.Interfaces;

public interface ILocalityValidationService
{
    Task<LocalityValidationResultDto> Validate(LocalityDto localityDto);
}