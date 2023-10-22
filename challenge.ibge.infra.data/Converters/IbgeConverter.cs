using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;

namespace challenge.ibge.infra.data.Converters;

public static class IbgeConverter
{
    public static LocalityDto ToDto(this Locality locality)
    {
        return new LocalityDto()
        {
            Id = locality.Id,
            Code = locality.IbgeCode,
            City = locality.City,
            UF = locality.UF
        };
    }
}