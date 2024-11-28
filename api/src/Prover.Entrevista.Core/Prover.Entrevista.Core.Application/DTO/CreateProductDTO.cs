namespace Prover.Entrevista.Core.Application.DTO;

public class CreateProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}