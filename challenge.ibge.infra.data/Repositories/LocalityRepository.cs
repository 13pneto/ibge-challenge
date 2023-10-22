using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Repositories.Interfaces;
using EFCore.BulkExtensions;

namespace challenge.ibge.infra.data.Repositories;

public class LocalityRepository : BaseRepository<Locality>, ILocalityRepository
{
    private readonly MySqlDbContext _mySqlDbContext;
    public LocalityRepository(MySqlDbContext mySqlDbContext) : base(mySqlDbContext)
    {
        _mySqlDbContext = mySqlDbContext;
    }

    public Task BulkInsertAsync(List<Locality> localities)
    {
        return _mySqlDbContext.BulkInsertAsync(localities);
    }
}