using challenge.ibge.core.Entities;
using challenge.ibge.core.Repositories.Interfaces;

namespace challenge.ibge.infra.data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MySqlDbContext mySqlDbContext) : base(mySqlDbContext)
    {
    }
}