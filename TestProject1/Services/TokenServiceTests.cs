﻿using AutoFixture;
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

public class TokenServiceTests : BaseTest, IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TokenService _tokenService;

    public TokenServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _tokenService = new TokenService();
    }

    [Fact]
    public async Task Generate_WithExpiresIn2Hours_ShouldGenerateToken()
    {
        //Arrange
        var userDto = Fixture.Create<UserDto>();
        
        //Action
        var result = _tokenService.Generate(userDto);

        //Assert
        result.Should().NotBeNull();
        result.Token.Should().StartWith("ey");
        result.ExpiresIn.Should().BeCloseTo(DateTimeOffset.UtcNow.AddHours(2), TimeSpan.FromSeconds(1));
    }

    public void Dispose()
    {
        //
    }
}