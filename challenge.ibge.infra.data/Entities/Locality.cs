using challenge.ibge.infra.data.Dtos;

namespace challenge.ibge.infra.data.Entities;

public class Locality : BaseEntity
{
    public int IbgeCode { get; private set; }
    public string City { get; private set; }
    public string UF { get; private set; }

    private Locality()
    {
        
    }

    public Locality(LocalityDto localityDto)
    {
        IbgeCode = localityDto.Code;
        City = localityDto.City;
        UF = localityDto.UF;
        
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(LocalityDto localityDto)
    {
        IbgeCode = localityDto.Code;
        City = localityDto.City;
        UF = localityDto.UF;
        
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}