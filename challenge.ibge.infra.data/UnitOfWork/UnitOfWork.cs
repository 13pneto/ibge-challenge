using challenge.ibge.infra.data.Repositories;
using challenge.ibge.infra.data.Repositories.Interfaces;
using challenge.ibge.infra.data.UnitOfWork.Interfaces;

namespace challenge.ibge.infra.data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly MySqlDbContext _mySqlDbContext;

    public UnitOfWork(MySqlDbContext mySqlDbContext)
    {
        _mySqlDbContext = mySqlDbContext;

        UserRepository = new UserRepository(mySqlDbContext);
        LocalityRepository = new LocalityRepository(mySqlDbContext);
    }

    public IUserRepository UserRepository { get; set; }
    public ILocalityRepository LocalityRepository { get; set; }


    public async Task SaveChangesAsync()
    {
        await _mySqlDbContext.SaveChangesAsync();
    }
}