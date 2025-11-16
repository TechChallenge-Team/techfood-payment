using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Payment.Application.Products.Dto;
using TechFood.Payment.Application.Products.Queries;

namespace TechFood.Payment.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler(IProductQueryProvider queries) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        => queries.GetByIdAsync(request.Id);
}
