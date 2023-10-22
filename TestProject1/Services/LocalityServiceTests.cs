using System.Linq.Expressions;
using AutoFixture;
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

public class LocalityServiceTests : BaseTest, IDisposable
{
    private readonly LocalityService _localityService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<LocalityService>> _loggerMock;
    private readonly Mock<ILocalityValidationService> _localityValidationServiceMock;

    public LocalityServiceTests()
    {
        _unitOfWorkMock = new();
        _loggerMock = new();
        _localityValidationServiceMock = new();
        
        
        _localityService = new LocalityService(_unitOfWorkMock.Object, _loggerMock.Object, 
            _localityValidationServiceMock.Object);
    }

    [Fact]
    public async Task Create()
    {
        //Arrange
        var localityDto = Fixture.Create<LocalityDto>();

        _unitOfWorkMock.Setup(x => x.LocalityRepository.Add(It.IsAny<Locality>()));
        
        //Action
        var result = await _localityService.CreateAsync(localityDto);

        //Assert
        _unitOfWorkMock.Verify(x => x.LocalityRepository.Add(It.Is<Locality>(x => 
            x.IbgeCode == localityDto.IbgeCode &&
            x.City == localityDto.City &&
            x.UF == localityDto.UF)), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        
        result.Should().BeEquivalentTo(localityDto, x => x.Excluding(y => y.Id));
    }

    [Fact]
    public async Task Update()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var localityDto = Fixture.Create<LocalityDto>();

        var dbLocality = Fixture.Create<Locality>();
        _unitOfWorkMock.Setup(x => x.LocalityRepository.FindByIdAsync(id))
            .ReturnsAsync(dbLocality);
        _unitOfWorkMock.Setup(x => x.LocalityRepository.Update(It.IsAny<Locality>()));
        
        //Action
        var result = await _localityService.UpdateAsync(id, localityDto);

        //Assert
        _unitOfWorkMock.Verify(x => x.LocalityRepository.Update(It.Is<Locality>(x => 
            x.IbgeCode == localityDto.IbgeCode &&
            x.City == localityDto.City &&
            x.UF == localityDto.UF &&
            x.Id == dbLocality.Id)
        ), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        
        result.Should().BeEquivalentTo(localityDto, x => x.Excluding(y => y.Id));
    }

    [Fact]
    public async Task Delete()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var dbLocality = Fixture.Create<Locality>();
        
        _unitOfWorkMock.Setup(x => x.LocalityRepository.FindByIdAsync(id))
            .ReturnsAsync(dbLocality);
        _unitOfWorkMock.Setup(x => x.LocalityRepository.Delete(It.IsAny<Locality>()));
        
        //Action
        await _localityService.DeleteAsync(id);

        //Assert
        _unitOfWorkMock.Verify(x => x.LocalityRepository.Delete(It.Is<Locality>(x => 
            x.Id == dbLocality.Id)
        ), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync()
    {
        //Arrange
        var id = Fixture.Create<int>();
        var dbLocality = Fixture.Create<Locality>();
        
        _unitOfWorkMock.Setup(x => x.LocalityRepository.GetByIdAsync(id))
            .ReturnsAsync(dbLocality);
        
        //Action
        await _localityService.GetByIdAsync(id);

        //Assert
        _unitOfWorkMock.Verify(x => x.LocalityRepository.GetByIdAsync(id), Times.Once);
    }
    
    [Fact]
    public async Task GetAllAsync()
    {
        //Arrange
        var dbLocalities = Fixture.CreateMany<Locality>().ToList();
        
        _unitOfWorkMock.Setup(x => x.LocalityRepository.GetAllAsync(It.IsAny<Expression<Func<Locality, bool>>>()))
            .ReturnsAsync(dbLocalities);
        
        //Action
        await _localityService.GetAllAsync(null);

        //Assert
        _unitOfWorkMock.Verify(x => x.LocalityRepository.GetAllAsync(It.IsAny<Expression<Func<Locality, bool>>>()),
            Times.Once);
    }

    public void Dispose()
    {
        _loggerMock.VerifyAll();
        _loggerMock.VerifyNoOtherCalls();
        
        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.VerifyNoOtherCalls();
        
        _localityValidationServiceMock.VerifyAll();
        _localityValidationServiceMock.VerifyNoOtherCalls();
    }
}