using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Payment.Application.Products.Dto;

namespace TechFood.Payment.Application.Products.Queries;

public interface IProductQueryProvider
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(Guid id);
}
