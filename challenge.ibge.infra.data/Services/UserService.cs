using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Converters;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.infra.data.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace challenge.ibge.infra.data.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptPasswordService _encryptPasswordService;

    public UserService(IUnitOfWork unitOfWork, IEncryptPasswordService encryptPasswordService)
    {
        _unitOfWork = unitOfWork;
        _encryptPasswordService = encryptPasswordService;
    }

    public async Task CreateAsync(CreateUserDto userDto)
    {
        var password = _encryptPasswordService.Encrypt(userDto.Password);
        userDto.Password = password;
        
        var user = new User(userDto);
        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UserDto userDto)
    {
        var dbUser = await _unitOfWork.UserRepository.FindByIdAsync(id);

        var isNeedUpdatePassword = userDto.Password != null;
        if (isNeedUpdatePassword)
        {
            userDto.Password = _encryptPasswordService.Encrypt(userDto.Password!);
        }
        
        dbUser.Update(userDto);
        
        _unitOfWork.UserRepository.Update(dbUser);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dbUser = await _unitOfWork.UserRepository.FindByIdAsync(id);
        
        _unitOfWork.UserRepository.Delete(dbUser);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<UserDto?> AuthenticateAsync(string email, string password)
    {
        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        if (dbUser is null)
        {
            throw new Exception($"Usuário com email {email} não encontrado");
        }
        
        var isCorrectPassword = _encryptPasswordService.Verify(password, dbUser.Password);
        return isCorrectPassword ? dbUser.ToDto() : null;
    }
}