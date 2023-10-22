using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Repositories.Interfaces;

namespace challenge.ibge.infra.data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MySqlDbContext mySqlDbContext) : base(mySqlDbContext)
    {
    }
}