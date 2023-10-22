using challenge.ibge.core.Dtos;
using challenge.ibge.core.Entities;

namespace challenge.ibge.core.Converters;

public static class IbgeConverter
{
    public static LocalityDto ToDto(this Locality locality)
    {
        return new LocalityDto()
        {
            Id = locality.Id,
            IbgeCode = locality.IbgeCode,
            City = locality.City,
            UF = locality.UF
        };
    }
}