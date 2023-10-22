namespace challenge.ibge.infra.data.Dtos;

public class LocalityValidationResultDto
{
    public int Row { get; set; }
    public bool IsValid { get; set; }
    public LocalityDto Locality { get; set; }
    public List<string> InvalidFields { get; set; } = new();
}