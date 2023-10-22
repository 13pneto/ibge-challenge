namespace challenge.ibge.core.Services.Interfaces;

public interface IEncryptPasswordService
{
    /// <summary>
    /// Encrypt an password.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>Return the encrypted password</returns>
    string Encrypt(string password);

    
    /// <summary>
    /// Verify if the password (non-encrypted) matches with passwordHashed
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHashed"></param>
    /// <returns></returns>
    bool Verify(string password, string passwordHashed);
}