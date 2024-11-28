using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;

namespace Prover.Entrevista.Core.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        if (products is null)
            throw new KeyNotFoundException($"Não existem produtos cadastrados.");

        var resultList = products.Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Quantity = p.Quantity,
            Price = p.Price
        });

        return resultList;
    }
        

    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        return new ProductDTO()
        { 
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Quantity = product.Quantity,
            Price = product.Price
        };
    }

    public async Task<Product> CreateAsync(CreateProductDTO createProduct)
    {
        if (string.IsNullOrEmpty(createProduct.Name))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (createProduct.Price <= 0)
            throw new ArgumentException("O preço do produto deve ser maior que zero.");

        var product = new Product()
        {
            Name = createProduct.Name,
            Description = createProduct.Description,
            Price = createProduct.Price,
            Quantity = createProduct.Quantity,
        };

        await _productRepository.CreateAsync(product);
        return product;
    }

    public async Task<Product> UpdateAsync(UpdateProductDTO updateProduct)
    {
        var existingProduct = await _productRepository.GetByIdAsync(updateProduct.Id);
        if (existingProduct is null)
            throw new KeyNotFoundException($"Produto com Id '{updateProduct.Id}' não encontrado.");

        if (string.IsNullOrEmpty(updateProduct.Name))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (updateProduct.Price <= 0)
            throw new ArgumentException("O preço do produto deve ser maior que zero.");

        var product = new Product()
        {
            Id = updateProduct.Id,
            Name = updateProduct.Name,
            Description = updateProduct.Description,
            Price = updateProduct.Price,
            Quantity = updateProduct.Quantity,
        };

        product.Update();
        await _productRepository.UpdateAsync(product);
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new KeyNotFoundException($"Produto com Id '{id}' não encontrado.");

        await _productRepository.DeleteAsync(id);
        return true;
    }
}