namespace challenge.ibge.authentication.Dtos;

public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    /// <summary>
    /// Keeps password null t o keep the password
    /// </summary>
    public string? Password { get; set; }
    
    public RoleEnum Role { get; set; }
}