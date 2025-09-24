using CoWorkingAccess.Domain.Common;

namespace CoWorkingAccess.Domain.Interfaces.Repositories;

public interface IGenericRepository<T>
{
    Task<Result<IEnumerable<T>>> GetAllAsync();
    
    Task<Result<T>> GetByIdAsync(int id);
    
    Task<Result<T>> CreateAsync(T entity);
    
    Task<Result> UpdateAsync(T entity);
    
    Task<Result> DeleteAsync(int id);
}