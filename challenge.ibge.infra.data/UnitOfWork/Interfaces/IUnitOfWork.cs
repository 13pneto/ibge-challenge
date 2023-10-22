using challenge.ibge.infra.data.Repositories.Interfaces;

namespace challenge.ibge.infra.data.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; set; } 
    ILocalityRepository LocalityRepository { get; set; } 
    
    Task SaveChangesAsync();
}