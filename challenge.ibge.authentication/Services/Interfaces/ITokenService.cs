using challenge.ibge.authentication.Dtos;

namespace challenge.ibge.authentication.Services.Interfaces;

public interface ITokenService
{
    TokenDto Generate(UserDto userDto);
}