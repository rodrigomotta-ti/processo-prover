﻿using System.ComponentModel.DataAnnotations;

namespace Prover.Entrevista.Core.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}