using challenge.ibge.infra.data.Services;

namespace challenge.ibge.infra.data.Dtos;

public class LocalityImportResult
{
    public TimeSpan Elapsed { get; set; }
    public int CreatedCount { get; set; }
    public int IgnoredCount { get; set; }
    public int FailedCount { get; set; }

    public List<LocalityValidationResultDto> FailedLocalities { get; set; }
}