using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Converters;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace challenge.ibge.infra.data.Services;

public class UserService : IUserService
{
    private readonly DbSet<User> _dbSet;
    private readonly MySqlDbContext _mySqlDbContext;
    private readonly IEncryptPasswordService _encryptPasswordService;

    public UserService(MySqlDbContext mySqlDbContext, IEncryptPasswordService encryptPasswordService)
    {
        _mySqlDbContext = mySqlDbContext;
        _encryptPasswordService = encryptPasswordService;
        _dbSet = mySqlDbContext.Users;
    }

    public async Task CreateAsync(CreateUserDto userDto)
    {
        var password = _encryptPasswordService.Encrypt(userDto.Password);
        userDto.Password = password;
        
        var user = new User(userDto);
        _dbSet.Add(user);
        await _mySqlDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UserDto userDto)
    {
        var dbUser = await _dbSet.SingleAsync(x => x.Id == id);
        dbUser.Update(userDto);
        
        _dbSet.Update(dbUser);
        await _mySqlDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dbUser = await _dbSet.SingleAsync(x => x.Id == id);
        
        _dbSet.Remove(dbUser);
        await _mySqlDbContext.SaveChangesAsync();
    }

    public async Task<UserDto?> AuthenticateAsync(string login, string password)
    {
        var dbUser = await _dbSet.FirstOrDefaultAsync(x => x.Email == login);
        if (dbUser is null)
        {
            throw new Exception("Usuário não encontrado");
        }
        
        var isCorrectPassword = _encryptPasswordService.Verify(password, dbUser.Password);
        return isCorrectPassword ? dbUser.ToDto() : null;
    }
}