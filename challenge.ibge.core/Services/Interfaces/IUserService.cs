using challenge.ibge.authentication.Dtos;

namespace challenge.ibge.core.Services.Interfaces;

public interface IUserService
{
    Task CreateAsync(CreateUserDto userDto);
    Task UpdateAsync(int id, UserDto userDto);
    Task DeleteAsync(int id);
    Task<UserDto?> AuthenticateAsync(string email, string password);
}