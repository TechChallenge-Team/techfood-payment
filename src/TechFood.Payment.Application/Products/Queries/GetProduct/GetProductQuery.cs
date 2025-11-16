using System;
using MediatR;
using TechFood.Payment.Application.Products.Dto;

namespace TechFood.Payment.Application.Products.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;
