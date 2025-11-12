using System;
using MediatR;
using TechFood.Application.Products.Dto;

namespace TechFood.Application.Products.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;
