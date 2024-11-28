using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<int> CreateAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(string tableName, int id);

    Task InsertList(IEnumerable<T> entities);
    Task UpdateBatch(IEnumerable<T> entities);
    Task RemoveBatch(IEnumerable<T> entities);
}