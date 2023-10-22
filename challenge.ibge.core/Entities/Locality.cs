using challenge.ibge.core.Dtos;

namespace challenge.ibge.core.Entities;

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
        IbgeCode = localityDto.IbgeCode;
        City = localityDto.City;
        UF = localityDto.UF;
        
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(LocalityDto localityDto)
    {
        IbgeCode = localityDto.IbgeCode;
        City = localityDto.City;
        UF = localityDto.UF;
        
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}