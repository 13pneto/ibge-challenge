using challenge.ibge.infra.data.Entities;

namespace challenge.ibge.infra.data.Repositories.Interfaces;

public interface ILocalityRepository : IBaseRepository<Locality>
{
    Task BulkInsertAsync(List<Locality> localities);
}