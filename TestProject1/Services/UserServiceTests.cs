using System.Linq.Expressions;
using AutoFixture;
using challenge.ibge.authentication;
using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.infra.data.UnitOfWork.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject1.Services;

public class UserServiceTests : BaseTest, IDisposable
{
    private readonly UserService _userService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEncryptPasswordService> _encryptedPasswordMock;

    public UserServiceTests()
    {
        _unitOfWorkMock = new();
        _encryptedPasswordMock = new();
        
        _userService = new UserService(_unitOfWorkMock.Object, _encryptedPasswordMock.Object);
    }

    [Fact]
    public async Task Create()
    {
        //Arrange
        var createUserDto = Fixture.Create<CreateUserDto>();
        
        _unitOfWorkMock.Setup(x => x.UserRepository.Add(It.IsAny<User>()));
        
        var encryptedPassword = Fixture.Create<string>();
        _encryptedPasswordMock.Setup(x => x.Encrypt(createUserDto.Password))
            .Returns(encryptedPassword)
            .Verifiable();
        
        //Action
        await _userService.CreateAsync(createUserDto);

        //Assert
        _unitOfWorkMock.Verify(x => x.UserRepository.Add(It.Is<User>(x => 
            x.Email == createUserDto.Email &&
            x.Password == encryptedPassword &&
            x.Role == RoleEnum.User
            )), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenPasswordNotUpdated_ShouldNotUpdatePassword()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var userDto = Fixture.Create<UserDto>();
        userDto.Password = null;
    
        var dbUser = Fixture.Create<User>();
        _unitOfWorkMock.Setup(x => x.UserRepository.FindByIdAsync(id))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.Update(It.IsAny<User>()));
        
        //Action
        await _userService.UpdateAsync(id, userDto);
    
        //Assert
        _encryptedPasswordMock.Verify(x => x.Encrypt(It.IsAny<string>()),
            Times.Never);
        
        _unitOfWorkMock.Verify(x => x.UserRepository.Update(It.Is<User>(x => 
            x.Email == userDto.Email &&
            x.Password == userDto.Password &&
            x.Role == dbUser.Role &&
            x.Id == dbUser.Id)
        ), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenPasswordWasUpdated_ShouldUpdatePassword()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var userDto = Fixture.Create<UserDto>();
        var newPasswordBeforeEncrypt = userDto.Password; 
    
        var dbUser = Fixture.Create<User>();
        _unitOfWorkMock.Setup(x => x.UserRepository.FindByIdAsync(id))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.Update(It.IsAny<User>()));

        var newEncryptedPassword = Fixture.Create<string>();
        _encryptedPasswordMock.Setup(x => x.Encrypt(newPasswordBeforeEncrypt))
            .Returns(newEncryptedPassword)
            .Verifiable();
        
        //Action
        await _userService.UpdateAsync(id, userDto);
    
        //Assert
        _encryptedPasswordMock.Verify(x => x.Encrypt(newPasswordBeforeEncrypt),
            Times.Once);
        
        _unitOfWorkMock.Verify(x => x.UserRepository.Update(It.Is<User>(x => 
            x.Email == userDto.Email &&
            x.Password == userDto.Password &&
            x.Role == dbUser.Role &&
            x.Id == dbUser.Id)
        ), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Delete()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var dbUser = Fixture.Create<User>();
        
        _unitOfWorkMock.Setup(x => x.UserRepository.FindByIdAsync(id))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.Delete(It.IsAny<User>()));
        
        //Action
        await _userService.DeleteAsync(id);
    
        //Assert
        _unitOfWorkMock.Verify(x => x.UserRepository.Delete(It.Is<User>(x => 
            x.Id == dbUser.Id)
        ), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task AuthenticateAsync_WhenUserNotExists_ShouldThrowException()
    {
        //Arrange
        var email = Fixture.Create<string>();
        var password = Fixture.Create<string>();

         _unitOfWorkMock.Setup(x => x.UserRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(() => null);
        
        //Action
        var act = () => _userService.AuthenticateAsync(email, password);
    
        //Assert
        await act.Should().ThrowExactlyAsync<Exception>().WithMessage($"Usuário com email {email} não encontrado");
    }
    
    [Fact]
    public async Task AuthenticateAsync_WhenPasswordIsCorrect_ShouldReturnUserDto()
    {
        //Arrange
        var email = Fixture.Create<string>();
        var password = Fixture.Create<string>();
        
        var dbUser = Fixture.Create<User>();

        _unitOfWorkMock.Setup(x => x.UserRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(dbUser);
        
        _encryptedPasswordMock.Setup(x => x.Verify(password, dbUser.Password))
            .Returns(true);
        
        //Action
        var result = await _userService.AuthenticateAsync(email, password);
    
        //Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task AuthenticateAsync_WhenPasswordIsIncorrect_ShouldReturnNull()
    {
        //Arrange
        var email = Fixture.Create<string>();
        var password = Fixture.Create<string>();
        var dbUser = Fixture.Create<User>();

        _unitOfWorkMock.Setup(x => x.UserRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(dbUser);
        
        _encryptedPasswordMock.Setup(x => x.Verify(password, dbUser.Password))
            .Returns(false);
        
        //Action
        var result = await _userService.AuthenticateAsync(email, password);
    
        //Assert
        result.Should().BeNull();
    }
    
    // [Fact]
    // public async Task GetAllAsync()
    // {
    //     //Arrange
    //     var dbLocalities = Fixture.CreateMany<Locality>().ToList();
    //     
    //     _unitOfWorkMock.Setup(x => x.LocalityRepository.GetAllAsync(It.IsAny<Expression<Func<Locality, bool>>>()))
    //         .ReturnsAsync(dbLocalities);
    //     
    //     //Action
    //     await _userService.GetAllAsync(null);
    //
    //     //Assert
    //     _unitOfWorkMock.Verify(x => x.LocalityRepository.GetAllAsync(It.IsAny<Expression<Func<Locality, bool>>>()),
    //         Times.Once);
    // }

    public void Dispose()
    {
        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.VerifyNoOtherCalls();
        
        _encryptedPasswordMock.VerifyAll();
        _encryptedPasswordMock.VerifyNoOtherCalls();
    }
}