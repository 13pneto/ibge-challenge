using challenge.ibge.authentication;
using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Dtos;

namespace challenge.ibge.infra.data.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public RoleEnum Role { get; private set; }
    public string Salt { get; private set; }

    private User()
    {
    }
    
    public User(CreateUserDto userDto)
    {
        Name = userDto.Name;
        Email = userDto.Email;
        Role = RoleEnum.User;
        
        Password = userDto.Password;
        Salt = "salt";
        
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(UserDto userDto)
    {
        Name = userDto.Name;
        Email = userDto.Email;
        Password = userDto.Password;
        
        UpdatedAt = CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateRole(RoleEnum role)
    {
        Role = role;
    }
}