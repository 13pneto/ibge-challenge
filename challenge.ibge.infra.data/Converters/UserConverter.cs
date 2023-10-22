using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Entities;

namespace challenge.ibge.infra.data.Converters;

public static class UserConverter
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto()
        {
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
}