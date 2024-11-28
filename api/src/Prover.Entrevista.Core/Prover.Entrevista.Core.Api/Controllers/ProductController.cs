using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("list")]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    [Produces("application/json")]
    public async Task<ActionResult<Product>> CreateProduct([FromQuery] CreateProductDTO product)
    {
        try
        {
            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update")]
    [Produces("application/json")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO product)
    {
        try
        {
            await _productService.UpdateAsync(product);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return Ok($"Produto deletado com sucesso.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}