using System;

namespace TechFood.Application.Products.Dto;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    Guid CategoryId,
    bool OutOfStock,
    string ImageUrl,
    decimal Price);
