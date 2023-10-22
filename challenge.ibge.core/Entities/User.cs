using challenge.ibge.authentication;
using challenge.ibge.authentication.Dtos;

namespace challenge.ibge.core.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public RoleEnum Role { get; private set; }

    private User()
    {
    }
    
    public User(CreateUserDto userDto)
    {
        Name = userDto.Name;
        Email = userDto.Email;
        Role = RoleEnum.User;
        
        Password = userDto.Password;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(UserDto userDto)
    {
        Name = userDto.Name;
        Email = userDto.Email;
        Password = userDto.Password;
        Role = userDto.Role;
        
        UpdatedAt = CreatedAt = DateTimeOffset.UtcNow;
    }
}