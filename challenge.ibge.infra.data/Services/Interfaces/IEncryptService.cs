namespace challenge.ibge.infra.data.Services.Interfaces;

public interface IEncryptService
{
    /// <summary>
    /// Encrypt an password.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>Return the encrypted password and salt</returns>
    string Encrypt(string password);

    bool Verify(string password, string passwordHashed);
}