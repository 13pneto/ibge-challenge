using challenge.ibge.core.Repositories.Interfaces;

namespace challenge.ibge.core;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; set; } 
    ILocalityRepository LocalityRepository { get; set; } 
    
    Task SaveChangesAsync();
}