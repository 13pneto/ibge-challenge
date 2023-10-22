using challenge.ibge.infra.data.Dtos;

namespace challenge.ibge.infra.data.Services.Interfaces;

public interface ILocalityValidationService
{
    Task<LocalityValidationResultDto> ValidateCanImport(LocalityDto localityDto);
}