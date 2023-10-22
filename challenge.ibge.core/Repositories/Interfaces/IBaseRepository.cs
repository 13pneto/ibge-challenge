using System.Linq.Expressions;

namespace challenge.ibge.core.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    void Add(T user);
    void Update(T user);
    void Delete(T user);
    Task<T?> GetByIdAsync(int id);
    Task<T> FindByIdAsync(int id);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null);
}