using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Data.Product;

namespace TechFood.Payment.Application.Common.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResult>> GetAllAsync();
    }
}
