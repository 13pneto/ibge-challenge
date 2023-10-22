using challenge.ibge.core.Entities;

namespace challenge.ibge.core.Repositories.Interfaces;

public interface ILocalityRepository : IBaseRepository<Locality>
{
    Task BulkInsertAsync(List<Locality> localities);
}