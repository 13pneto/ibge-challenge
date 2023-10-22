using challenge.ibge.authentication.Dtos;
using challenge.ibge.core.Entities;

namespace challenge.ibge.core.Converters;

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