using AutoFixture;
using challenge.ibge.authentication.Dtos;
using challenge.ibge.authentication.Services;
using challenge.ibge.infra.data;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace TestProject1.Services;

public class EncryptPasswordServiceTests : BaseTest, IDisposable
{
    private readonly EncryptPasswordService _encryptPasswordService;

    public EncryptPasswordServiceTests()
    {
        _encryptPasswordService = new EncryptPasswordService();
    }

    [Fact]
    public async Task Encrypt()
    {
        //Arrange
        var password = Fixture.Create<string>();
        
        //Action
        var result = _encryptPasswordService.Encrypt(password);

        //Assert
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Verify_WhenPasswordMatches_ShouldReturnTrue()
    {
        //Arrange
        var password = Fixture.Create<string>();
        var encryptedPassword = _encryptPasswordService.Encrypt(password);
        
        //Action
        var result = _encryptPasswordService.Verify(password, encryptedPassword);

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task Verify_WhenPasswordNOTMatches_ShouldReturnFalse()
    {
        //Arrange
        var password = Fixture.Create<string>();
        var encryptedPassword = _encryptPasswordService.Encrypt(password);
        var invalidPassword = $"{password}1";
        
        //Action
        var result = _encryptPasswordService.Verify(invalidPassword, encryptedPassword);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateEqualsPasswordGenerateDifferentEncryptedPassword()
    {
        //Arrange
        var password1 = Fixture.Create<string>();
        var password2 = Fixture.Create<string>();
        var encryptedPassword1 = _encryptPasswordService.Encrypt(password1);
        var encryptedPassword2 = _encryptPasswordService.Encrypt(password2);
        
        //Action
        var resultPassword1 = _encryptPasswordService.Verify(password1, encryptedPassword1);
        var resultPassword2 = _encryptPasswordService.Verify(password2, encryptedPassword2);

        //Assert
        encryptedPassword1.Should().NotBeEquivalentTo(encryptedPassword2);

        resultPassword1.Should().BeTrue();
        resultPassword2.Should().BeTrue();
    }

    public void Dispose()
    {
        //
    }
}