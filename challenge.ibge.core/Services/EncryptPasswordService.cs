using challenge.ibge.core.Services.Interfaces;

namespace challenge.ibge.core.Services;

public class EncryptPasswordService : IEncryptPasswordService
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHashed);
    }
}