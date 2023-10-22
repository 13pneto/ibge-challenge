using AutoFixture;
using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject1.Services;

public class LocalityServiceTests : BaseTest, IDisposable
{
    private readonly LocalityService _localityService;
    private readonly Mock<MySqlDbContext> _mySqlDbContextMock;
    private readonly Mock<DbSet<Locality>> _dbSetMock;
    private readonly Mock<ILogger<LocalityService>> _loggerMock;
    private readonly Mock<ILocalityValidationService> _localityValidationServiceMock;

    public LocalityServiceTests()
    {
        _mySqlDbContextMock = new();
        _loggerMock = new();
        _localityValidationServiceMock = new();

        _dbSetMock = new();
        _mySqlDbContextMock.SetupGet(x => x.Localities)
            .Returns(_dbSetMock.Object);
        
        _localityService = new LocalityService(_mySqlDbContextMock.Object, _loggerMock.Object, 
            _localityValidationServiceMock.Object);
    }

    [Fact]
    public async Task Create()
    {
        //Arrange
        var localityDto = Fixture.Create<LocalityDto>();
        
        _mySqlDbContextMock.Setup(x => x.Localities.AddAsync(It.IsAny<Locality>(), default));
        
        
        //Action
        var result = await _localityService.CreateAsync(localityDto);

        //Assert
        _dbSetMock.Verify(x => It.Is<Locality>(x => 
            x.UF == localityDto.UF &&
            x.IbgeCode == localityDto.Code &&
            x.City == localityDto.City
            ), Times.Once);
        
        _mySqlDbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        
        result.Should().BeEquivalentTo(localityDto);
    }

    public void Dispose()
    {
        _loggerMock.VerifyAll();
        _loggerMock.VerifyNoOtherCalls();
        
        _mySqlDbContextMock.VerifyAll();
        _mySqlDbContextMock.VerifyNoOtherCalls();
        
        _localityValidationServiceMock.VerifyAll();
        _localityValidationServiceMock.VerifyNoOtherCalls();
    }
}