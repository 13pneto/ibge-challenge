using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using challenge.ibge.authentication.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace challenge.ibge.authentication.Services;

public static class TokenService
{
    public static TokenDto GenerateToken(UserDto userDto)
    {
        var secret = "MUIxMDM3MzQtNTFDOC00QkQ1LTkzNDctN0MxRjlGQjVGODM5MzY2ODQ1NjUtODg4Qi00NkNBLThGRkYtNjQ2RkI0QzlCQTFE";
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.Role.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new TokenDto()
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresIn = (DateTimeOffset)tokenDescriptor.Expires
        };
    }
}