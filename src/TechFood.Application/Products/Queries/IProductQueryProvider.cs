using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Application.Products.Dto;

namespace TechFood.Application.Products.Queries;

public interface IProductQueryProvider
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(Guid id);
}
