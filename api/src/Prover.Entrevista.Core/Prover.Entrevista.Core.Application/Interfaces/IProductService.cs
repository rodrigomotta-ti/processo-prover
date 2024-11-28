using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    Task<ProductDTO> GetByIdAsync(int id);
    Task<Product> CreateAsync(CreateProductDTO product);
    Task<Product> UpdateAsync(UpdateProductDTO product);
    Task<bool> DeleteAsync(int id);
}