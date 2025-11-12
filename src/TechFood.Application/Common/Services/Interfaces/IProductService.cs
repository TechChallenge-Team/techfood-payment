using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Application.Common.Data.Product;

namespace TechFood.Application.Common.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResult>> GetAllProducts();
    }
}
