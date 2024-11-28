using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Domain.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<int> DeleteAsync(int id);
}