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

public class LocalityValidationServiceTests : BaseTest, IDisposable
{
    private readonly LocalityValidationService _localityValidationService;

    public LocalityValidationServiceTests()
    {
        _localityValidationService = new LocalityValidationService();
    }

    [Fact]
    public async Task ValidateCanImport_WhenAllFieldAreValid_ShouldReturnLocalityValidationResultDtoWithIsValidTrue()
    {
        //Arrange
        var localityDto = new LocalityDto()
        {
            IbgeCode = 1021459,
            UF = "PR",
            City = "Curitiba"
        };
        
        //Action
        var result = await _localityValidationService.ValidateCanImport(localityDto);

        //Assert
        result.IsValid.Should().BeTrue();
        result.InvalidFields.Should().BeEmpty();
        result.Row.Should().Be(default);
        result.Locality.Should().BeEquivalentTo(localityDto);
    }

    [Fact]
    public async Task ValidateCanImport_WhenAllFieldsAreInvalid_ShouldReturnLocalityValidationResultDtoWithIsValidFalseWithAndFieldsAsInvalid()
    {
        //Arrange
        var localityDto = new LocalityDto()
        {
            IbgeCode = 12345678,
            UF = "PRR",
            City = "Ab"
        };
        
        //Action
        var result = await _localityValidationService.ValidateCanImport(localityDto);

        //Assert
        result.IsValid.Should().BeFalse();
        result.InvalidFields.Should().BeEquivalentTo(new List<string>
        {
            nameof(localityDto.IbgeCode), nameof(localityDto.UF), nameof(localityDto.City)
        });
        result.Row.Should().Be(default);
        result.Locality.Should().BeEquivalentTo(localityDto);
    }

    [Theory]
    [InlineData(123456)]
    [InlineData(12345678)]
    [InlineData(0)]
    public async Task ValidateCanImport_WhenOnlyCodeIsInvalid_ShouldReturnLocalityValidationResultDtoWithIsValidFalseWithOnlyCodeFieldAsInvalid(
        int ibgeCode)
    {
        //Arrange
        var localityDto = new LocalityDto()
        {
            IbgeCode = ibgeCode,
            UF = "PR",
            City = "Curitiba"
        };
        
        //Action
        var result = await _localityValidationService.ValidateCanImport(localityDto);

        //Assert
        result.IsValid.Should().BeFalse();
        result.InvalidFields.Should().BeEquivalentTo(new List<string> {nameof(localityDto.IbgeCode)});
        result.Row.Should().Be(default);
        result.Locality.Should().BeEquivalentTo(localityDto);
    }

    [Theory]
    [InlineData("Ab")]
    [InlineData("A-B-C")]
    [InlineData("Sa-o")]
    [InlineData(null)]
    public async Task ValidateCanImport_WhenOnlyCityIsInvalid_ShouldReturnLocalityValidationResultDtoWithIsValidFalseWithOnlyCityFieldAsInvalid(
        string city)
    {
        //Arrange
        var localityDto = new LocalityDto()
        {
            IbgeCode = 1234567,
            UF = "PR",
            City = city
        };
        
        //Action
        var result = await _localityValidationService.ValidateCanImport(localityDto);

        //Assert
        result.IsValid.Should().BeFalse();
        result.InvalidFields.Should().BeEquivalentTo(new List<string> {nameof(localityDto.City)});
        result.Row.Should().Be(default);
        result.Locality.Should().BeEquivalentTo(localityDto);
    }

    [Theory]
    [InlineData("P")]
    [InlineData("PRR")]
    [InlineData("")]
    [InlineData(null)]
    public async Task ValidateCanImport_WhenOnlyUFIsInvalid_ShouldReturnLocalityValidationResultDtoWithIsValidFalseWithOnlyUFFieldAsInvalid(
        string uf)
    {
        //Arrange
        var localityDto = new LocalityDto()
        {
            IbgeCode = 1234567,
            UF = uf,
            City = "Curitiba"
        };
        
        //Action
        var result = await _localityValidationService.ValidateCanImport(localityDto);

        //Assert
        result.IsValid.Should().BeFalse();
        result.InvalidFields.Should().BeEquivalentTo(new List<string> {nameof(localityDto.UF)});
        result.Row.Should().Be(default);
        result.Locality.Should().BeEquivalentTo(localityDto);
    }

    public void Dispose()
    {
        //
    }
}