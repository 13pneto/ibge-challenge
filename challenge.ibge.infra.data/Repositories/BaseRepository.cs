using System.Linq.Expressions;
using challenge.ibge.core.Entities;
using challenge.ibge.core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace challenge.ibge.infra.data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
{
    private DbSet<T> _dbSet;
    
    public BaseRepository(MySqlDbContext mySqlDbContext)
    {
        _dbSet = mySqlDbContext.Set<T>();
    }
    
    public void Add(T user)
    {
        _dbSet.Add(user);
    }

    public void Update(T user)
    {
        _dbSet.Update(user);
    }

    public void Delete(T user)
    {
        _dbSet.Remove(user);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var dbUser = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
        return dbUser;
    }

    public async Task<T> FindByIdAsync(int id)
    {
        var dbUser = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
        if (dbUser is null)
        {
            throw new Exception($"{nameof(User)} with id {id} not found in database");
        }
        return dbUser;
    }

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        return filter is null ? _dbSet.ToListAsync() : _dbSet.Where(filter).ToListAsync();
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
    {
        return _dbSet.FirstOrDefaultAsync(filter);
    }
}