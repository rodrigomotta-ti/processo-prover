using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;
using Prover.Entrevista.Core.Infrastructure.Interfaces.Context;

namespace Prover.Entrevista.Core.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private const string _tableName = "products";

        public ProductRepository(IDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteAsync(int id) => await DeleteAsync(_tableName, id);
    }
}