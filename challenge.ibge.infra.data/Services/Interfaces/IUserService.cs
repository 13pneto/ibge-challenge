using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Dtos;

namespace challenge.ibge.infra.data.Services.Interfaces;

public interface IUserService
{
    Task CreateAsync(CreateUserDto userDto);
    Task UpdateAsync(int id, UserDto userDto);
    Task DeleteAsync(int id);
    Task<UserDto?> AuthenticateAsync(string login, string password);
}