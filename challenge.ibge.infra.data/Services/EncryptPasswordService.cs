using challenge.ibge.infra.data.Services.Interfaces;

namespace challenge.ibge.infra.data.Services;

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