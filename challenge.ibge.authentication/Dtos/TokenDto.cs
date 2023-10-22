namespace challenge.ibge.authentication.Dtos;

public class TokenDto
{
    public string Token { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }
}